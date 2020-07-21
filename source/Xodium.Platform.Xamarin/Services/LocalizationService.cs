using System.Globalization;
using System.Resources;
using Xodium.Services;

namespace Xodium.Platform.Xamarin.Services
{
    public class LocalizationService : ILocalizationService
    {
        private readonly ResourceManager resourceManager;

        public LocalizationService(ResourceManager resourceManager)
        {
            this.resourceManager = resourceManager;
        }

        public string GetString(string key)
        {
            return resourceManager.GetString(key, CultureInfo.CurrentCulture) ?? key;
        }
    }
}
