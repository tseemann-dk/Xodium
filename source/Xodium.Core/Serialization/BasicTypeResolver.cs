using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Xodium.Serialization
{
    [Flags]
    public enum BasicTypeResolverOptions
    {
        None = 0,
        UseAssembly = 1,
        UseFullName = 2,
        UseAssemblyAndFullName = UseAssembly | UseFullName
    }

    public class BasicTypeResolver : ITypeResolver
    {
        private readonly IReadOnlyList<Type> knownTypes;
        private readonly BasicTypeResolverOptions options;

        public BasicTypeResolver(AppDomain appDomain, BasicTypeResolverOptions options = BasicTypeResolverOptions.UseAssemblyAndFullName)
            : this(appDomain.GetAssemblies().SelectMany(x => x.GetTypes()), options)
        {
        }

        public BasicTypeResolver(Assembly assembly, BasicTypeResolverOptions options = BasicTypeResolverOptions.UseAssemblyAndFullName)
            : this(assembly?.GetTypes(), options)
        {
        }

        public BasicTypeResolver(IEnumerable<Type> knownTypes, BasicTypeResolverOptions options = BasicTypeResolverOptions.UseAssemblyAndFullName)
        {
            this.knownTypes = knownTypes?.ToList() ?? new List<Type>();
            this.options = options;
        }

        public Type ResolveType(string assemblyName, string typeName)
        {
            return knownTypes.FirstOrDefault(x => TypeMatches(x, assemblyName, typeName));
        }

        public bool UnresolveType(Type type, out string assemblyName, out string typeName)
        {
            assemblyName = IsUseAssemblyEnabled ? type.Assembly.GetName().Name : null;
            typeName = IsUseFullNameEnabled ? type.FullName : type.Name;
            return knownTypes.Contains(type);
        }

        private bool TypeMatches(Type type, string assemblyName, string typeName)
        {
            var assemblyMatches = !IsUseAssemblyEnabled || string.Compare(assemblyName, type.Assembly.GetName().Name, StringComparison.OrdinalIgnoreCase) == 0;
            var nameMatches = string.Compare(typeName, IsUseFullNameEnabled ? type.FullName : type.Name, StringComparison.Ordinal) == 0;
            return assemblyMatches && nameMatches;
        }

        private bool IsUseAssemblyEnabled => HasOption(BasicTypeResolverOptions.UseAssembly);
        private bool IsUseFullNameEnabled => HasOption(BasicTypeResolverOptions.UseFullName);

        private bool HasOption(BasicTypeResolverOptions option) => (options & option) == option;
    }
}
