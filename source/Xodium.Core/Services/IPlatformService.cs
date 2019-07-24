namespace Xodium.Services
{
    public static class PlatformTypes
    {
        public const string Unknown = "Unknown";
        public const string Android = "Android";
        public const string iOS = "iOS";
        public const string Win32 = "Win32";
        public const string UWP = "UWP";
        public const string WPF = "WPF";
        public const string macOS = "macOS";
    }

    public interface IPlatformService
    {
        string PlatformType { get; }

        string AppName { get; }
        string AppDescription { get; }
        string AppVersion { get; }

        string OperatingSystemName { get; }
        string OperatingSystemVersion { get; }
    }
}
