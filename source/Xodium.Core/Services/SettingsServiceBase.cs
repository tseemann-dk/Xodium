using System;
using System.Collections.Generic;
using System.Linq;

namespace Xodium.Services
{
    public abstract class SettingsServiceBase : ISettingsService
    {
        private IDictionary<string, object> values;

        protected SettingsServiceBase(IDictionary<string, object> values)
        {
            this.values = values ?? throw new ArgumentNullException(nameof(values));
        }

        private string BuildKey(string key, string section) => string.IsNullOrEmpty(section) ? key : $"{section}.{key}";
        private bool KeyIsInSection(string key, string section) => key.StartsWith(section + ".");

        private object GetObject(string key, string section)
        {
            return values.TryGetValue(BuildKey(key, section), out object result) ? result : null;
        }

        private T GetValue<T>(string key, T defaultValue, string section)
        {
            var obj = GetObject(key, section);
            return obj == null ? defaultValue : (T)obj;
        }

        private void SetObject(string key, object value, string section)
        {
            if (value != null && value.Equals(GetValue(key, section))) return;
            values[BuildKey(key, section)] = value;
        }

        private bool SetValue<T>(string key, T value, string section)
        {
            SetObject(key, value, section);
            return true;
        }

        public void Clear(string section = null)
        {
            if (section == null)
            {
                values.Clear();
                return;
            }

            var keys = values.Keys.Where(key => KeyIsInSection(key, section)).ToList();

            foreach (var key in keys)
            {
                values.Remove(key);
            }
        }

        public bool Contains(string key, string section = null) => values.ContainsKey(BuildKey(key, section));

        public decimal GetValue(string key, decimal defaultValue, string section = null) => GetValue<decimal>(key, defaultValue, section);
        public Guid GetValue(string key, Guid defaultValue, string section = null) => GetValue<Guid>(key, defaultValue, section);
        public DateTime GetValue(string key, DateTime defaultValue, string section = null) => GetValue<DateTime>(key, defaultValue, section);
        public float GetValue(string key, float defaultValue, string section = null) => GetValue<float>(key, defaultValue, section);
        public int GetValue(string key, int defaultValue, string section = null) => GetValue<int>(key, defaultValue, section);
        public string GetValue(string key, string defaultValue, string section = null) => GetValue<string>(key, defaultValue, section);
        public long GetValue(string key, long defaultValue, string section = null) => GetValue<long>(key, defaultValue, section);
        public bool GetValue(string key, bool defaultValue, string section = null) => GetValue<bool>(key, defaultValue, section);
        public double GetValue(string key, double defaultValue, string section = null) => GetValue<double>(key, defaultValue, section);

        public bool SetValue(string key, bool value, string section = null) => SetValue<bool>(key, value, section);
        public bool SetValue(string key, double value, string section = null) => SetValue<double>(key, value, section);
        public bool SetValue(string key, Guid value, string section = null) => SetValue<Guid>(key, value, section);
        public bool SetValue(string key, DateTime value, string section = null) => SetValue<DateTime>(key, value, section);
        public bool SetValue(string key, float value, string section = null) => SetValue<float>(key, value, section);
        public bool SetValue(string key, int value, string section = null) => SetValue<int>(key, value, section);
        public bool SetValue(string key, string value, string section = null) => SetValue<string>(key, value, section);
        public bool SetValue(string key, long value, string section = null) => SetValue<long>(key, value, section);
        public bool SetValue(string key, decimal value, string section = null) => SetValue<decimal>(key, value, section);

        public void Remove(string key, string section = null) => values.Remove(BuildKey(key, section));
    }
}
