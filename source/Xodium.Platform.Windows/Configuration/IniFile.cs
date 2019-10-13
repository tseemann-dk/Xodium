using System.Runtime.InteropServices;
using System.Text;

namespace Xodium.Platform.Windows.Configuration
{
    public class IniFile
    {
        private readonly string path;

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern long WritePrivateProfileString(string section, string key, string value, string filePath);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern int GetPrivateProfileString(string section, string key, string defaultValue, StringBuilder value, int size, string filePath);

        public IniFile(string path)
        {
            this.path = path ?? throw new System.ArgumentNullException(nameof(path));
        }

        public string ReadValue(string section, string key)
        {
            var result = new StringBuilder(255);
            GetPrivateProfileString(section, key, "", result, 255, path);
            return result.ToString();
        }

        public void WriteValue(string section, string key, string value)
        {
            WritePrivateProfileString(section, key, value, path);
        }

        public void DeleteKey(string section, string key)
        {
            WriteValue(key, null, section);
        }

        public void DeleteSection(string section)
        {
            WriteValue(null, null, section);
        }

        public bool KeyExists(string section, string key)
        {
            return ReadValue(key, section).Length > 0;
        }
    }
}
