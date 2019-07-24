namespace Xodium.Services
{
    public static class DeviceTypes
    {
        public const string Unknown = "Unknown";
        public const string Phone = "Phone";
        public const string Tablet = "Tablet";
        public const string Desktop = "Desktop";
        public const string Watch = "Watch";
        public const string Television = "Television";
    }

    public interface IDeviceService
    {
        string DeviceType { get; }
        string DeviceId { get; }
        DisplayMetrics DisplayMetrics { get; }
    }
}
