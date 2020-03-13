using Newtonsoft.Json.Serialization;
using System;

namespace Xodium.Serialization.Json.Newtonsoft
{
    public class TypeAwareSerializationBinder : DefaultSerializationBinder
    {
        private readonly ITypeResolver typeResolver;

        public TypeAwareSerializationBinder(ITypeResolver typeResolver)
        {
            this.typeResolver = typeResolver ?? throw new ArgumentNullException(nameof(typeResolver));
        }

        public override void BindToName(Type serializedType, out string assemblyName, out string typeName)
        {
            typeResolver.UnresolveType(serializedType, out assemblyName, out typeName);
        }

        public override Type BindToType(string assemblyName, string typeName)
        {
            return typeResolver.ResolveType(assemblyName, typeName) ?? base.BindToType(assemblyName, typeName);
        }
    }
}
