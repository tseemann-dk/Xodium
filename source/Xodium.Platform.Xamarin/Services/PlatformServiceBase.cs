using Xamarin.Forms;
using Xodium.Services;

namespace Xodium.Platform.Xamarin.Services
{
    public abstract class PlatformServiceBase : IPlatformService
    {
        public string PlatformType
        {
            get
            {
                switch (Device.RuntimePlatform)
                {
                    case Device.Android:
                        return PlatformTypes.Android;
                    case Device.iOS:
                        return PlatformTypes.iOS;
                    case Device.UWP:
                        return PlatformTypes.UWP;
                    case Device.WPF:
                        return PlatformTypes.WPF;
                    case Device.macOS:
                        return PlatformTypes.macOS;
                    default:
                        return PlatformTypes.Unknown;
                }
            }
        }

        public abstract string AppName { get; }
        public abstract string AppDescription { get; }
        public abstract string AppVersion { get; }
        public abstract string OperatingSystemName { get; }
        public abstract string OperatingSystemVersion { get; }
    }
}
