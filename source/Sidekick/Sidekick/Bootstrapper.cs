using System;
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
    public delegate IStore<TState> StoreProvider<TState>(Reducer<TState> reducer, TState state, Redux.Middleware<TState>[] middlewares);

    public class Bootstrapper : BootstrapperBase
    {
        private readonly StoreProvider<AppState> storeProvider;

        public Bootstrapper(StoreProvider<AppState> storeProvider = null)
        {
            StoreProvider<AppState> p = (reducer, state, middlewares) =>
                new ReduxStore<AppState>(r => new Redux.Store<AppState>(r, state, middlewares), reducer);

            this.storeProvider = storeProvider ?? p;
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
                StoreConfiguration.Middlewares);

            registry.RegisterInstance(Store);
            registry.RegisterInstance<IStore>(Store);
        }
    }
}
