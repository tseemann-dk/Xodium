using System.Collections.Generic;

namespace Xodium.Platform.Windows.Services
{
    public interface IRegistryKey
    {
        int ValueCount { get; }

        void DeleteValue(string name);
        object GetValue(string name);
        ICollection<string> GetValueNames();
        void SetValue(string name, object value);
    }
}
