using Foundation;
using Xodium.Platform.Xamarin.Services;

namespace Xodium.Platform.iOS.Services
{
    public class PlatformService : PlatformServiceBase
    {
        public override string AppName => NSBundle.MainBundle.InfoDictionary["CFBundleDisplayName"].ToString();
        public override string AppDescription => string.Empty;
        public override string AppVersion => NSBundle.MainBundle.InfoDictionary["CFBundleVersion"].ToString();
        public override string OperatingSystemName => "iOS"; // TODO
        public override string OperatingSystemVersion => ""; // TODO
    }
}
