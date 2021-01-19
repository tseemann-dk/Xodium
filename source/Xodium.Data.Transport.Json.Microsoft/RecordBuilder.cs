using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.Json;
using Xodium.Data.Schemas;

namespace Xodium.Data.Transport.Json.Microsoft
{
    class RecordBuilder
    {
        private readonly ISchema schema;
        private readonly IDictionary<string, JsonElement> cache = new Dictionary<string, JsonElement>();

        public RecordBuilder(ISchema schema)
        {
            this.schema = schema ?? throw new ArgumentNullException(nameof(schema));
        }

        public IDataRecord BuildRecord(JsonElement graph)
        {
            cache.Clear();

            var values =
                from field in schema.Fields
                let property = GetProperty(graph, field.Name)
                select GetValue(property, field);

            return new DataRecord(schema, values);
        }

        private JsonElement GetProperty(JsonElement obj, string path)
        {
            var dot = path.IndexOf(".");

            if (dot < 0)
            {
                if (obj.ValueKind != JsonValueKind.Object)
                {
                    return default;
                }

                if (!obj.TryGetProperty(path, out var property))
                {
                    throw new KeyNotFoundException($"Property \"{path}\" not found in {obj.GetRawText()}");
                }

                return property;
            }

            obj = GetObject(obj, path[..dot]);
            return obj.ValueKind == JsonValueKind.Object ? GetProperty(obj, path[(dot + 1)..]) : default;
        }

        private JsonElement GetObject(JsonElement root, string path)
        {
            if (cache.TryGetValue(path, out var obj))
            {
                return obj;
            }

            var property = GetProperty(root, path);

            obj = property.ValueKind switch
            {
                JsonValueKind.Null => default,
                JsonValueKind.Object => property,
                _ => throw new ArgumentException($"Property \"{path}\" is not an object", nameof(path)),
            };

            cache.Add(path, obj);
            return obj;
        }

        private object GetValue(JsonElement property, IField field)
        {
            switch (property.ValueKind)
            {
                case JsonValueKind.String:
                    return field.Parse(property.GetString());
                case JsonValueKind.Number:
                    return field is DoubleField
                        ? field.Parse(property.GetDouble().ToString())
                        : field.Parse(property.GetInt64().ToString());
                case JsonValueKind.True:
                case JsonValueKind.False:
                    return field.Parse(property.GetBoolean().ToString());
                case JsonValueKind.Null:
                case JsonValueKind.Undefined:
                    return null;
                default:
                    throw new ArgumentOutOfRangeException(nameof(property),
                        $"Unsupported value: {property.ValueKind} ({property.GetRawText()})");
            }
        }
    }
}
