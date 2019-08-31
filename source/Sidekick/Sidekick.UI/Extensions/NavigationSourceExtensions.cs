using System;
using System.Threading.Tasks;
using Sidekick.State;
using Xodium.Flow;
using Xodium.Mvvm;

namespace Sidekick.UI.Extensions
{
    public static class NavigationSourceExtensions
    {
        public static AppState GetAppState(this INavigationSource self) => 
            self.ExecutionEnvironment.GetService<IStore>().GetState() as AppState;

        public static Task HandleException(this INavigationSource self, Exception exception) => 
            self.ExecutionEnvironment.DialogService.DisplayException("Error", exception);
    }
}
