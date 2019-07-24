using System;
using System.Reflection;
using Xodium.Services;

namespace Xodium.Platform.Windows.Services
{
    public class PlatformService : IPlatformService
    {
        public string PlatformType => PlatformTypes.Win32;

        public string AppName => Assembly.GetExecutingAssembly()?.GetName()?.Name;

        public string AppDescription => string.Empty;

        public string AppVersion => Assembly.GetExecutingAssembly().GetName()?.Version.ToString();

        public string OperatingSystemName => Environment.OSVersion.Platform.ToString();

        public string OperatingSystemVersion => Environment.OSVersion.VersionString;
    }
}
