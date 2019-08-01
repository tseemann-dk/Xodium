using Sidekick.Models;

namespace Sidekick.UWP
{
    public class UwpBootstrapper : Bootstrapper
    {
        public UwpBootstrapper(StoreProvider<AppState> storeProvider)
            : base(storeProvider)
        {
        }
    }
}
