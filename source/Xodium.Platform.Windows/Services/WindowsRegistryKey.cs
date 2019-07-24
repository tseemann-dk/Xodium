using Microsoft.Win32;
using System;
using System.Collections.Generic;

namespace Xodium.Platform.Windows.Services
{
    public class WindowsRegistryKey : IRegistryKey
    {
        private readonly RegistryKey key;

        public WindowsRegistryKey(RegistryKey key)
        {
            this.key = key ?? throw new ArgumentNullException(nameof(key));
        }

        public int ValueCount => key.ValueCount;

        public void DeleteValue(string name) => key.DeleteValue(name, false);
        public object GetValue(string name) => key.GetValue(name);
        public ICollection<string> GetValueNames() => key.GetValueNames();
        public void SetValue(string name, object value) => key.SetValue(name, value);
    }
}
