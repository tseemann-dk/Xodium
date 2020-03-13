using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Xodium.Serialization.Json.Newtonsoft
{
    public class TypeAwareJsonConverter : JsonConverter
    {
        private readonly Type baseType;
        private readonly ITypeResolver typeResolver;
        private readonly string discriminator;

        public TypeAwareJsonConverter(Type baseType, ITypeResolver typeResolver, string discriminator = null)
        {
            this.baseType = baseType ?? throw new ArgumentNullException(nameof(baseType));
            this.typeResolver = typeResolver ?? throw new ArgumentNullException(nameof(typeResolver));
            this.discriminator = discriminator ?? "type";
        }

        public override bool CanRead => true;
        public override bool CanWrite => false;
        public override bool CanConvert(Type type) => baseType.IsAssignableFrom(type);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jObject = JObject.Load(reader);
            var typeName = jObject[discriminator]?.Value<string>();
            var type = typeResolver.ResolveType(null, typeName);

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
