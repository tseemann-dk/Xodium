using System;

namespace Xodium.Serialization
{
    public interface ITypeResolver
    {
        bool UnresolveType(Type type, out string assemblyName, out string typeName);
        Type ResolveType(string assemblyName, string name);
    }
}
