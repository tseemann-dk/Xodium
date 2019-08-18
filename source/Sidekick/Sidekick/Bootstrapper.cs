using System;
using Redux;
using Sidekick.Features.Shopper.ViewModels;
using Sidekick.Features.Shopper.Views;
using Sidekick.State;
using Xodium.Flow;
using Xodium.Injection;
using Xodium.Mvvm;
using Xodium.Platform.Xamarin;
using Xodium.Platform.Xamarin.Services;
using Xodium.Redux;

namespace Sidekick
{
    public delegate IStore<T> StoreProvider<T>(Reducer<T> reducer, T state, Middleware<T>[] middlewares);

    public class Bootstrapper : BootstrapperBase
    {
        private readonly StoreProvider<AppState> storeProvider;

        public Bootstrapper(StoreProvider<AppState> storeProvider = null)
        {
            this.storeProvider = storeProvider ?? ((reducer, state, middlewares) => 
                new Store<AppState>(reducer, state, middlewares));
        }

        public IStore<AppState> Store { get; private set; }

        protected override IExecutionEnvironment CreateExecutionEnvironment(Func<IDependencyResolver> resolver)
            => new XamarinExecutionEnvironment(resolver);

        protected override void RegisterServices(IDependencyRegistry registry)
        {
            base.RegisterServices(registry);

            RegisterStore(registry);

            registry.RegisterFactory<INavigationService>(resolver => new NavigationService(App.NavigationPage, () => ViewRegistry));
            registry.RegisterFactory<IDialogService>(resolver => new DialogService(App.NavigationPage, () => ViewRegistry));
        }

        protected override void RegisterViews(IViewRegistry registry)
        {
            base.RegisterViews(registry);

            registry.RegisterViewType<ComponentLookupView, ComponentLookupViewModel>();
        }

        private void RegisterStore(IDependencyRegistry registry)
        {
            Store = storeProvider(
                AppStateReducer.Execute, 
                AppStateGenerator.GenerateSampleState(), 
                AppStateReducer.Middlewares);

            registry.RegisterInstance(Store);
            registry.RegisterInstance<IActionDispatcher>(new ReduxDispatcher<AppState>(Store));
        }
    }
}
