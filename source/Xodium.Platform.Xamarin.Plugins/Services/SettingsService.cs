using System;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using Xodium.Services;

namespace Xodium.Platform.Xamarin.Services
{
    public class SettingsService : ISettingsService
    {
        private static ISettings Settings => CrossSettings.Current ?? throw new NullReferenceException("No settings");
        private static string GetSectionFileName(string section) => string.IsNullOrEmpty(section) ? null : section + ".settings";

        public void Clear(string section = null) => Settings.Clear(section);
        public bool Contains(string key, string section = null) => Settings.Contains(key, section);

        public decimal GetValue(string key, decimal defaultValue, string section = null) => Settings.GetValueOrDefault(key, defaultValue, GetSectionFileName(section));
        public Guid GetValue(string key, Guid defaultValue, string section = null) => Settings.GetValueOrDefault(key, defaultValue, GetSectionFileName(section));
        public DateTime GetValue(string key, DateTime defaultValue, string section = null) => Settings.GetValueOrDefault(key, defaultValue, GetSectionFileName(section));
        public float GetValue(string key, float defaultValue, string section = null) => Settings.GetValueOrDefault(key, defaultValue, GetSectionFileName(section));
        public int GetValue(string key, int defaultValue, string section = null) => Settings.GetValueOrDefault(key, defaultValue, GetSectionFileName(section));
        public string GetValue(string key, string defaultValue, string section = null) => Settings.GetValueOrDefault(key, defaultValue, GetSectionFileName(section));
        public long GetValue(string key, long defaultValue, string section = null) => Settings.GetValueOrDefault(key, defaultValue, GetSectionFileName(section));
        public bool GetValue(string key, bool defaultValue, string section = null) => Settings.GetValueOrDefault(key, defaultValue, GetSectionFileName(section));
        public double GetValue(string key, double defaultValue, string section = null) => Settings.GetValueOrDefault(key, defaultValue, GetSectionFileName(section));

        public bool SetValue(string key, bool value, string section = null) => Settings.AddOrUpdateValue(key, value, GetSectionFileName(section));
        public bool SetValue(string key, double value, string section = null) => Settings.AddOrUpdateValue(key, value, GetSectionFileName(section));
        public bool SetValue(string key, Guid value, string section = null) => Settings.AddOrUpdateValue(key, value, GetSectionFileName(section));
        public bool SetValue(string key, DateTime value, string section = null) => Settings.AddOrUpdateValue(key, value, GetSectionFileName(section));
        public bool SetValue(string key, float value, string section = null) => Settings.AddOrUpdateValue(key, value, GetSectionFileName(section));
        public bool SetValue(string key, int value, string section = null) => Settings.AddOrUpdateValue(key, value, GetSectionFileName(section));
        public bool SetValue(string key, string value, string section = null) => Settings.AddOrUpdateValue(key, value, GetSectionFileName(section));
        public bool SetValue(string key, long value, string section = null) => Settings.AddOrUpdateValue(key, value, GetSectionFileName(section));
        public bool SetValue(string key, decimal value, string section = null) => Settings.AddOrUpdateValue(key, value, GetSectionFileName(section));

        public void Remove(string key, string section = null) => Settings.Remove(key, GetSectionFileName(section));
    }
}
