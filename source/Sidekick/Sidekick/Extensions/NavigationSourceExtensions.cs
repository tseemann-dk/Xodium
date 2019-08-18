using System;
using System.Threading.Tasks;
using Redux;
using Sidekick.State;
using Xodium.Mvvm;

namespace Sidekick.Extensions
{
    public static class NavigationSourceExtensions
    {
        public static AppState GetAppState(this INavigationSource self) => 
            self.ExecutionEnvironment.GetService<IStore<AppState>>().GetState();

        public static Task HandleException(this INavigationSource self, Exception exception) => 
            self.ExecutionEnvironment.DialogService.DisplayException("Error", exception);
    }
}
