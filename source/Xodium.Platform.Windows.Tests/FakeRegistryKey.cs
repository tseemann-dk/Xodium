using System.Collections.Generic;
using Xodium.Platform.Windows.Services;

namespace Xodium.Platform.Windows.Tests
{
    class FakeRegistryKey : IRegistryKey
    {
        private readonly Dictionary<string, object> values = new Dictionary<string, object>();

        public int ValueCount => values.Count;

        public void DeleteValue(string name) => values.Remove(name);
        public object GetValue(string name) => values.TryGetValue(name, out var value) ? value : null;
        public ICollection<string> GetValueNames() => values.Keys;
        public void SetValue(string name, object value) => values[name] = value;
    }
}
