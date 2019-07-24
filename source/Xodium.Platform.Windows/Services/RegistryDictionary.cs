using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Xodium.Platform.Windows.Services
{
    public class RegistryDictionary : IDictionary<string, object>
    {
        private readonly IRegistryKey registryKey;

        public RegistryDictionary(IRegistryKey registryKey)
        {
            this.registryKey = registryKey;
        }

        public object this[string name]
        {
            get => registryKey.GetValue(name);
            set => registryKey.SetValue(name, value);
        }

        public ICollection<string> Keys => registryKey.GetValueNames();
        public ICollection<object> Values => GetItems().Select(kv => kv.Value).ToList();
        public int Count => registryKey.ValueCount;
        public bool IsReadOnly => false;

        public void Add(string key, object value)
        {
            registryKey.SetValue(key, value);
        }

        public void Add(KeyValuePair<string, object> item)
        {
            Add(item.Key, item.Value);
        }

        public void Clear()
        {
            foreach (var item in GetItems())
            {
                registryKey.DeleteValue(item.Key);
            }
        }

        public bool Contains(KeyValuePair<string, object> item)
        {
            return this[item.Key] == item.Value;
        }

        public bool ContainsKey(string key)
        {
            return this[key] != null;
        }

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator() => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return GetItems().GetEnumerator();
        }

        private IEnumerable<KeyValuePair<string, object>> GetItems()
        {
            var values =
                from name in registryKey.GetValueNames()
                let value = registryKey.GetValue(name)
                select new KeyValuePair<string, object>(name, value);

            return values.ToList();
        }

        public bool Remove(string key)
        {
            registryKey.DeleteValue(key);
            return true;
        }

        public bool Remove(KeyValuePair<string, object> item)
        {
            return Remove(item.Key);
        }

        public bool TryGetValue(string key, out object value)
        {
            if (registryKey.GetValueNames().Contains(key))
            {
                value = this[key];
                return true;
            }

            value = null;
            return false;
        }
    }
}
