using System;

namespace Xodium.Serialization
{
    public interface ITypeResolver
    {
        Type ResolveType(string assemblyName, string typeName);
        bool UnresolveType(Type type, out string assemblyName, out string typeName);
    }
}
