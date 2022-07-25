using System;
using Sidekick.Reducers;
using Sidekick.Shopper.Models;
using Sidekick.State;
using Xodium.Flow;
using Xodium.Injection;
using Xodium.Mvvm;
using Xodium.Platform.Xamarin;
using Xodium.Redux;

namespace Sidekick
{
    public delegate IStore<TState> StoreProvider<TState>(Reducer<TState> reducer, TState state, Redux.Middleware<TState>[] middlewares);

    public class AppBootstrapper : BootstrapperBase
    {
        private readonly StoreProvider<AppState> storeProvider;

        public AppBootstrapper(StoreProvider<AppState> storeProvider = null)
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

            //registry.RegisterSingleton<IShop>(new Shopper.eBay.eBayShop());
            registry.RegisterSingleton<IShop>(new Shopper.Flickr.FlickrShop());
        }

        private void RegisterStore(IDependencyRegistry registry)
        {
            var middlewares = new[]
            {
                Shopper.Middleware.ComponentLookupMiddleware.CreateMiddleware(),
                Shopper.Middleware.ShoppingListMiddleware.CreateMiddleware()
            };

            Store = storeProvider(
                AppStateReducer.Reduce, 
                AppStateGenerator.GenerateSampleState(), 
                middlewares);

            registry.RegisterSingleton(Store);
            registry.RegisterSingleton<IStore>(Store);
        }
    }
}
