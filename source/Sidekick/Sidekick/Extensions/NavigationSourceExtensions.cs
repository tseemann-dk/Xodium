using Redux;
using Sidekick.Models;
using Xodium.Mvvm;

namespace Sidekick.Extensions
{
    public static class NavigationSourceExtensions
    {
        public static AppState GetAppState(this INavigationSource self) => self.ExecutionEnvironment.GetService<IStore<AppState>>().GetState();
    }
}
