using System;
using Xodium.Services;

namespace Xodium.Platform.Windows.Services
{
    public class DeviceService : IDeviceService
    {
        public DeviceService()
        {
            DisplayMetrics = new DisplayMetrics(
                System.Windows.SystemParameters.PrimaryScreenWidth,
                System.Windows.SystemParameters.PrimaryScreenHeight,
                1.0f);
        }

        public string DeviceType => DeviceTypes.Desktop;
        public string DeviceId => Environment.MachineName;
        public DisplayMetrics DisplayMetrics { get; }
    }
}
