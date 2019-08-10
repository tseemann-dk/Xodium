using Windows.ApplicationModel.Resources.Core;
using Xodium.Services;

namespace Xodium.Platform.Uwp.Services
{
    public class LocalizationService : ILocalizationService
    {
        private readonly ResourceContext resourceContext;
        private readonly ResourceMap resourceMap;

        public LocalizationService(string resourceMapReference)
        {
            resourceContext = ResourceContext.GetForViewIndependentUse();
            resourceMap = ResourceManager.Current.MainResourceMap.GetSubtree(resourceMapReference);
        }

        public string GetString(string key)
        {
            return resourceMap?.GetValue(key, resourceContext)?.ValueAsString ?? key;
        }
    }
}
