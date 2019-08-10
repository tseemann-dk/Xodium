using Xodium.Services;
using ApplicationModel = Windows.ApplicationModel;

namespace Xodium.Platform.Uwp.Services
{
    public class PlatformService : IPlatformService
    {
        public string PlatformType => PlatformTypes.UWP;

        public string AppName => ApplicationModel.Package.Current.DisplayName;
        public string AppDescription => ApplicationModel.Package.Current.Description;

        public string AppVersion
        {
            get
            {
                var version = ApplicationModel.Package.Current.Id.Version;
                return $"{version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
            }
        }

        public string OperatingSystemName => "Windows 10"; // TODO
        public string OperatingSystemVersion => string.Empty; // TODO
    }
}
