using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Xodium.Serialization.Json.CoreFX
{
    public class TypeAwareJsonConverter<T> : JsonConverter<T>
        where T : ITypeDiscriminated
    {
        private readonly IEnumerable<Type> types;

        public TypeAwareJsonConverter()
        {
            types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(t => typeof(T).IsAssignableFrom(t) && t.IsClass && !t.IsAbstract)
                .ToList();
        }

        public override bool CanConvert(Type typeToConvert) => typeof(T).IsAssignableFrom(typeToConvert);

        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
                throw new JsonException($"Expected token type {nameof(JsonTokenType.StartObject)} but found {reader.TokenType}");

            using (var jsonDocument = JsonDocument.ParseValue(ref reader))
            {
                var discriminator = nameof(ITypeDiscriminated.TypeDiscriminator);

                if (!jsonDocument.RootElement.TryGetProperty(discriminator, out var typeProperty))
                    throw new JsonException($"Property \"{discriminator}\" was not found");

                var typeName = typeProperty.GetString();
                var type = types.FirstOrDefault(x => x.Name == typeName);

                if (type == null)
                    throw new JsonException($"Unknown type \"{typeName}\"");

                var jsonObject = jsonDocument.RootElement.GetRawText();
                var result = (T)JsonSerializer.Deserialize(jsonObject, type);

                return result;
            }
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            var properties = value.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(x => x.GetCustomAttribute<JsonIgnoreAttribute>(true) == null)
                .Select(x => new
                {
                    Property = x,
                    Name = x.GetCustomAttribute<JsonPropertyNameAttribute>(true)?.Name ?? ToCamelCase(x.Name)
                })
                .OrderBy(x => GetPropertyOrderKey(x.Property, x.Name))
                .ToList();

            var values = (
                from p in properties
                let v = p.Property.GetValue(value)
                where v != null
                select (p, v)
                ).ToDictionary(x => x.p.Name, x => x.v);

            JsonSerializer.Serialize(writer, values, options);
        }

        protected virtual object GetPropertyOrderKey(PropertyInfo property, string name)
        {
            return null;
        }

        private static string ToCamelCase(string value) =>
            string.IsNullOrEmpty(value) ? value : char.ToLowerInvariant(value.First()) + value.Substring(1);
    }
}
