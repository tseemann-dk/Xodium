using Xodium.Services;

namespace Xodium.Platform.Windows.Services
{
    public class SettingsService : SettingsServiceBase
    {
        public SettingsService(IRegistryKey registryKey)
            : base(new RegistryDictionary(registryKey))
        {
        }
    }
}
