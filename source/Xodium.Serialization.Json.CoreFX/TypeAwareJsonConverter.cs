using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Xodium.Serialization.Json.CoreFX
{
    public class TypeAwareJsonConverter<T> : JsonConverter<T>
    {
        public TypeAwareJsonConverter(ITypeResolver typeResolver, string typeDiscriminator = null)
        {
            TypeResolver = typeResolver ?? throw new ArgumentNullException(nameof(typeResolver));
            TypeDiscriminator = typeDiscriminator ?? "type";
        }

        public ITypeResolver TypeResolver { get; }
        public string TypeDiscriminator { get; }

        public override bool CanConvert(Type typeToConvert) => typeof(T).IsAssignableFrom(typeToConvert);

        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException($"Expected object start but found {reader.TokenType}");
            }

            if (!reader.Read() || reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException($"Expected property but found {reader.TokenType}");
            }

            var name = reader.GetString();
            if (name != TypeDiscriminator)
            {
                throw new JsonException($"Expected type discriminator \"{TypeDiscriminator}\" but found \"{name}\"");
            }

            if (!reader.Read() || reader.TokenType != JsonTokenType.String)
            {
                throw new JsonException($"Expected type discriminator string value but found {reader.TokenType}");
            }

            var typeName = reader.GetString();
            var type = TypeResolver.ResolveType(null, typeName);

            if (type == null)
                throw new InvalidOperationException($"Unknown type name \"{typeName}\"");

            var result = (T)Activator.CreateInstance(type);

            var properties = type
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(x => x.CanWrite)
                .ToDictionary(x => ToPropertyName(x.Name, options));

            // The discriminator token is consumed above (before this dictionary even exists) purely
            // to resolve which CLR type to instantiate, so it's never revisited by the loop below -
            // bind it onto a matching writable property here too, the same way every other property
            // is bound, so a type whose discriminator-backing property isn't guaranteed to equal
            // Activator.CreateInstance's default (e.g. GetType().Name) still ends up with the wire's
            // actual value instead of that default.
            if (properties.TryGetValue(TypeDiscriminator, out var discriminatorProperty))
            {
                discriminatorProperty.SetValue(result, typeName);
            }

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return result;
                }

                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    var propertyName = reader.GetString();
                    var property = properties.TryGetValue(propertyName, out var p) ? p : null;
                    var value = JsonSerializer.Deserialize(ref reader, property?.PropertyType ?? typeof(object), options);

                    if (property?.CanWrite ?? false)
                    {
                        property.SetValue(result, value);
                    }
                }
            }

            throw new JsonException("Unexpected end of stream");
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            var properties = value.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(x => ShouldWriteProperty(x, options))
                .Select(x => new
                {
                    Property = x,
                    Name = x.GetCustomAttribute<JsonPropertyNameAttribute>(true)?.Name ?? ToPropertyName(x.Name, options)
                })
                .OrderBy(x => GetPropertyOrder(x.Property, x.Name))
                .ToList();

            var values = (
                from p in properties
                let v = p.Property.GetValue(value)
                where v != null
                select (p, v)
                ).ToDictionary(x => x.p.Name, x => x.v);

            JsonSerializer.Serialize(writer, values, options);
        }

        protected bool CanWriteProperty(PropertyInfo property, JsonSerializerOptions options)
        {
            return !options.IgnoreReadOnlyProperties || property.CanWrite;
        }

        protected virtual object GetPropertyOrder(PropertyInfo property, string name)
        {
            return !IsDiscriminator(property);
        }

        protected bool IsDiscriminator(PropertyInfo property)
        {
            return string.Compare(property.Name, TypeDiscriminator, true, CultureInfo.InvariantCulture) == 0;
        }

        protected virtual bool ShouldWriteProperty(PropertyInfo property, JsonSerializerOptions options)
        {
            return IsDiscriminator(property) ||
                (!ShouldIgnoreProperty(property) && CanWriteProperty(property, options));
        }

        protected bool ShouldIgnoreProperty(PropertyInfo property)
        {
            return property.GetCustomAttribute<JsonIgnoreAttribute>(true) != null;
        }

        protected string ToPropertyName(string name, JsonSerializerOptions options)
        {
            return options.PropertyNamingPolicy?.ConvertName(name) ?? name;
        }
    }
}
