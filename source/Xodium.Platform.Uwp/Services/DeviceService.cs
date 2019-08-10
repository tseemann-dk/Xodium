using System.Linq;
using Xodium.Platform.Uwp.Triggers;
using Xodium.Services;

namespace Xodium.Platform.Uwp.Services
{
    public class DeviceService : IDeviceService
    {
        public DeviceService()
        {
            var info = Windows.Graphics.Display.DisplayInformation.GetForCurrentView();
            DisplayMetrics = new DisplayMetrics(info.ScreenWidthInRawPixels, info.ScreenHeightInRawPixels, info.RawPixelsPerViewPixel);
        }

        public string DeviceType
        {
            get
            {
                switch (DeviceFormFactorTrigger.GetFormFactor())
                {
                    case DeviceFormFactor.Phone:
                        return DeviceTypes.Phone;
                    case DeviceFormFactor.Tablet:
                        return DeviceTypes.Tablet;
                    case DeviceFormFactor.Desktop:
                        return DeviceTypes.Desktop;
                    default:
                        return DeviceTypes.Unknown;
                }
            }
        }

        public string DeviceId
        {
            get
            {
                if (!Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.System.Profile.HardwareIdentification"))
                    return null;

                var token = Windows.System.Profile.HardwareIdentification.GetPackageSpecificToken(null);
                var id = token.Id;
                var reader = Windows.Storage.Streams.DataReader.FromBuffer(id);
                var bytes = new byte[id.Length];
                var result = string.Empty;

                reader.ReadBytes(bytes);
                return bytes.Aggregate(result, (s, b) => s + b.ToString());
            }
        }

        public DisplayMetrics DisplayMetrics { get; }
    }
}
