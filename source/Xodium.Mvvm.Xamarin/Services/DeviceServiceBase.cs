using Xamarin.Forms;
using Xodium.Services;

namespace Xodium.Platform.Xamarin.Services
{
    public abstract class DeviceServiceBase : IDeviceService
    {
        public string DeviceType
        {
            get
            {
                switch (Device.Idiom)
                {
                    case TargetIdiom.Phone:
                        return DeviceTypes.Phone;
                    case TargetIdiom.Tablet:
                        return DeviceTypes.Tablet;
                    case TargetIdiom.Desktop:
                        return DeviceTypes.Desktop;
                    case TargetIdiom.TV:
                        return DeviceTypes.Television;
                    default:
                        return DeviceTypes.Unknown;
                }
            }
        }

        public abstract string DeviceId { get; }
        public abstract DisplayMetrics DisplayMetrics { get; }
    }
}
