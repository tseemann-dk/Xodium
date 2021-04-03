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
                throw new JsonException($"Expected token type {nameof(JsonTokenType.StartObject)} but found {reader.TokenType}");

            using (var jsonDocument = JsonDocument.ParseValue(ref reader))
            {
                if (!jsonDocument.RootElement.TryGetProperty(TypeDiscriminator, out var typeProperty))
                    throw new JsonException($"Type discriminator property \"{TypeDiscriminator}\" was not found");

                var typeName = typeProperty.GetString();
                var type = TypeResolver.ResolveType(null, typeName);

                if (type == null)
                    throw new JsonException($"Cannot resolve type \"{typeName}\"");

                var jsonObject = jsonDocument.RootElement.GetRawText();
                var result = (T)JsonSerializer.Deserialize(jsonObject, type);

                return result;
            }
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            var properties = value.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(x => ShouldWriteProperty(x, options))
                .Select(x => new
                {
                    Property = x,
                    Name = x.GetCustomAttribute<JsonPropertyNameAttribute>(true)?.Name ?? GetPropertyName(x, options)
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

        protected virtual bool ShouldWriteProperty(PropertyInfo property, JsonSerializerOptions options)
        {
            return IsDiscriminator(property) ||
                (!ShouldIgnoreProperty(property) && CanWriteProperty(property, options));
        }

        protected bool ShouldIgnoreProperty(PropertyInfo property)
        {
            return property.GetCustomAttribute<JsonIgnoreAttribute>(true) != null;
        }

        protected bool CanWriteProperty(PropertyInfo property, JsonSerializerOptions options)
        {
            return !options.IgnoreReadOnlyProperties || property.CanWrite;
        }

        protected bool IsDiscriminator(PropertyInfo property)
        {
            return string.Compare(property.Name, TypeDiscriminator, true, CultureInfo.InvariantCulture) == 0;
        }

        protected virtual string GetPropertyName(PropertyInfo property, JsonSerializerOptions options)
        {
            return options.PropertyNamingPolicy?.ConvertName(property.Name) ?? property.Name;
        }

        protected virtual object GetPropertyOrder(PropertyInfo property, string name)
        {
            return !IsDiscriminator(property);
        }
    }
}
