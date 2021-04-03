using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Xodium.Serialization.Json.Newtonsoft
{
    public class TypeAwareJsonConverter<T> : JsonConverter
    {
        public TypeAwareJsonConverter(ITypeResolver typeResolver, string typeDiscriminator = null)
        {
            TypeResolver = typeResolver ?? throw new ArgumentNullException(nameof(typeResolver));
            TypeDiscriminator = typeDiscriminator ?? "type";
        }

        public ITypeResolver TypeResolver { get; }
        public string TypeDiscriminator { get; }

        public override bool CanRead => true;
        public override bool CanWrite => false;

        public override bool CanConvert(Type type) => typeof(T).IsAssignableFrom(type);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jObject = JObject.Load(reader);
            var typeName = jObject[TypeDiscriminator]?.Value<string>();
            var type = TypeResolver.ResolveType(null, typeName);

            if (type == null)
                throw new InvalidOperationException($"Unknown type name \"{typeName}\"");

            var result = Activator.CreateInstance(type);
            serializer.Populate(jObject.CreateReader(), result);
            return result;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotSupportedException();
        }
    }
}
