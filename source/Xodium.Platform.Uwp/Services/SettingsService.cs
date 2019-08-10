using Xodium.Services;

namespace Xodium.Platform.Uwp.Services
{
    public class LocalSettingsService : SettingsServiceBase
    {
        public LocalSettingsService()
            : base(Windows.Storage.ApplicationData.Current.LocalSettings.Values)
        {
        }
    }

    public class RoamingSettingsService : SettingsServiceBase
    {
        public RoamingSettingsService()
            : base(Windows.Storage.ApplicationData.Current.RoamingSettings.Values)
        {
        }
    }
}
