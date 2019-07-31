using System;

namespace Xodium.Productivity.Microsoft365.Utilities
{
    public class ExtendedProperty
    {
        public ExtendedProperty(string id, string value)
        {
            ParseId(id);
            Value = value;
        }

        public ExtendedProperty(string typeName, string namespaceId, string propertyName, string value)
        {
            TypeName = typeName ?? throw new ArgumentNullException(nameof(typeName));
            NamespaceId = namespaceId ?? throw new ArgumentNullException(nameof(namespaceId));
            PropertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
            Value = value;
        }

        public string TypeName { get; private set; }
        public string NamespaceId { get; private set; }
        public string PropertyName { get; private set; }
        public string Value { get; private set; }

        public string Id => $"{TypeName} {NamespaceId} Name {PropertyName}";

        public override string ToString()
        {
            return $"[{Id}] = {Value}";
        }

        private void ParseId(string id)
        {
            var parts = id.Split();

            if (parts.Length < 4 || (parts.Length >= 2 && parts[2].ToLower() != "name"))
                throw new ArgumentException($"Invalid property id: {id}\nShould be: '<TypeName> <NamespaceId> Name <PropertyName>'", nameof(id));

            TypeName = parts[0];
            NamespaceId = parts[1];
            PropertyName = parts[3];
        }
    }
}
