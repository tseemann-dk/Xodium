namespace Xodium.Productivity.Microsoft365.Utilities
{
    public class StringProperty : ExtendedProperty
    {
        public StringProperty(string namespaceId, string propertyName, string value) 
            : base("String", namespaceId, propertyName, value)
        {
        }
    }
}
