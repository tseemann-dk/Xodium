using Sidekick.Models;

namespace Sidekick.Droid
{
    public class AndroidBootstrapper : Bootstrapper
    {
        public AndroidBootstrapper(StoreProvider<AppState> storeProvider = null) 
            : base(storeProvider)
        {
        }
    }
}