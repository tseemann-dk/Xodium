using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Xodium.Data.Schemas;

namespace Xodium.Data.Transport.Json.Newtonsoft
{
    class RecordBuilder
    {
        private readonly ISchema schema;
        private readonly IDictionary<string, JObject> cache = new Dictionary<string, JObject>();

        public RecordBuilder(ISchema schema)
        {
            this.schema = schema ?? throw new ArgumentNullException(nameof(schema));
        }

        public IDataRecord BuildRecord(JObject obj)
        {
            cache.Clear();

            var values =
                from field in schema.Fields
                let property = GetProperty(obj, field.Name)
                select GetValue(property, field);

            return new DataRecord(schema, values);
        }

        private JProperty GetProperty(JObject obj, string path)
        {
            var dot = path.IndexOf(".");

            if (dot < 0)
            {
                return obj.Property(path);
            }

            obj = GetObject(obj, path[..dot]);
            return obj == null ? null : GetProperty(obj, path[(dot + 1)..]);
        }

        private JObject GetObject(JObject root, string path)
        {
            if (cache.TryGetValue(path, out var obj))
            {
                return obj;
            }

            var property = GetProperty(root, path);

            obj = property.Value.Type switch
            {
                JTokenType.Null => null,
                JTokenType.Object => property.Value.Value<JObject>(),
                _ => throw new ArgumentException($"Property \"{path}\" is not an object", nameof(path)),
            };

            cache.Add(path, obj);
            return obj;
        }

        private object GetValue(JProperty property, IField field)
        {
            if (property == null)
                return null;

            switch (property.Value.Type)
            {
                case JTokenType.String:
                    return field.Parse(property.Value.Value<string>());
                case JTokenType.Float:
                case JTokenType.Integer:
                    return field is DoubleField
                        ? field.Parse(property.Value.Value<double>().ToString())
                        : field.Parse(property.Value.Value<long>().ToString());
                case JTokenType.Boolean:
                    return field.Parse(property.Value.Value<bool>().ToString());
                case JTokenType.Null:
                case JTokenType.Undefined:
                    return null;
                default:
                    throw new ArgumentOutOfRangeException(nameof(property),
                        $"Unsupported value: {property.Value.Type} ({property.Value.Value<string>()})");
            }
        }
    }
}
