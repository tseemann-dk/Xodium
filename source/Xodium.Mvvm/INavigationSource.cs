using System;
using System.Threading.Tasks;
using Xodium.Flow;

namespace Xodium.Mvvm
{
    public interface INavigationSource
    {
        IExecutionEnvironment ExecutionEnvironment { get; }
    }

    public static class NavigationSourceExtensions
    {
        public static void DispatchAction(this INavigationSource self, IAction action) 
            => self.ExecutionEnvironment.Store.Dispatch(action);

        public static Task DispatchActionsAsync<T>(this INavigationSource self, ActionsCreator<T> actionsCreator) 
            => self.ExecutionEnvironment.GetService<IStore<T>>().DispatchAsync(actionsCreator);

        public static Task GoBack(this INavigationSource self) 
            => self.ExecutionEnvironment.NavigationService.GoBack();

        public static Task GoBackToRoot(this INavigationSource self) 
            => self.ExecutionEnvironment.NavigationService.GoBackToRoot();

        public static Task GoTo(this INavigationSource self, INavigationSource destination) 
            => self.ExecutionEnvironment.NavigationService.GoTo(destination);

        public static Task OpenModal(this INavigationSource self, INavigationSource destination) 
            => self.ExecutionEnvironment.NavigationService.OpenModal(destination);

        public static Task OpenPopup(this INavigationSource self, INavigationSource destination) 
            => self.ExecutionEnvironment.NavigationService.OpenPopup(destination);

        public static Task OpenUri(this INavigationSource self, Uri uri) 
            => self.ExecutionEnvironment.NavigationService.OpenUri(uri);
    }
}
