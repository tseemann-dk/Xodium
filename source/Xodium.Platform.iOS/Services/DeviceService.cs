using UIKit;
using Xodium.Platform.Xamarin.Services;
using Xodium.Services;

namespace Xodium.Platform.iOS.Services
{
    public class DeviceService : DeviceServiceBase
    {
        public DeviceService()
        {
            var screen = UIScreen.MainScreen;
            DisplayMetrics = new DisplayMetrics(screen.Bounds.Width, screen.Bounds.Height, screen.Scale);
        }

        public override string DeviceId => UIDevice.CurrentDevice.IdentifierForVendor.AsString();
        public override DisplayMetrics DisplayMetrics { get; }
    }
}
