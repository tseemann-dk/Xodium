using Sidekick.Models;

namespace Sidekick.iOS
{
    public class iOSBootstrapper : Bootstrapper
    {
        public iOSBootstrapper(StoreProvider<AppState> storeProvider = null) 
            : base(storeProvider)
        {
        }
    }
}