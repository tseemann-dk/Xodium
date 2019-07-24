using System;

namespace Xodium.Services
{
    public interface ISettingsService
    {
        void Clear(string section = null);
        bool Contains(string key, string section = null);

        decimal GetValue(string key, decimal defaultValue, string section = null);
        Guid GetValue(string key, Guid defaultValue, string section = null);
        DateTime GetValue(string key, DateTime defaultValue, string section = null);
        float GetValue(string key, float defaultValue, string section = null);
        int GetValue(string key, int defaultValue, string section = null);
        string GetValue(string key, string defaultValue, string section = null);
        long GetValue(string key, long defaultValue, string section = null);
        bool GetValue(string key, bool defaultValue, string section = null);
        double GetValue(string key, double defaultValue, string section = null);

        bool SetValue(string key, bool value, string section = null);
        bool SetValue(string key, double value, string section = null);
        bool SetValue(string key, Guid value, string section = null);
        bool SetValue(string key, DateTime value, string section = null);
        bool SetValue(string key, float value, string section = null);
        bool SetValue(string key, int value, string section = null);
        bool SetValue(string key, string value, string section = null);
        bool SetValue(string key, long value, string section = null);
        bool SetValue(string key, decimal value, string section = null);

        void Remove(string key, string fileName = null);
    }
}
