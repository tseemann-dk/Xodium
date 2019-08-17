using System;
using Redux;
using Sidekick.State;
using Xodium.Flow;
using Xodium.Injection;
using Xodium.Mvvm;
using Xodium.Platform.Xamarin;
using Xodium.Redux;

namespace Sidekick
{
    public delegate IStore<T> StoreProvider<T>(Reducer<T> reducer, T state);

    public class Bootstrapper : BootstrapperBase
    {
        private readonly StoreProvider<AppState> storeProvider;

        public Bootstrapper(StoreProvider<AppState> storeProvider = null)
        {
            this.storeProvider = storeProvider ?? ((reducer, state) => new Store<AppState>(reducer, state));
        }

        public IStore<AppState> Store { get; private set; }

        protected override IExecutionEnvironment GetExecutionEnvironment(Func<IDependencyResolver> resolver)
            => new XamarinExecutionEnvironment(resolver);

        protected override void RegisterServices(IDependencyRegistry registry)
        {
            base.RegisterServices(registry);

            RegisterStore(registry);
        }

        private void RegisterStore(IDependencyRegistry registry)
        {
            Store = storeProvider(AppStateReducer.Execute, AppStateGenerator.GenerateAppState());

            registry.RegisterInstance(Store);
            registry.RegisterInstance<IActionDispatcher>(new ReduxDispatcher<AppState>(Store));
        }
    }
}
