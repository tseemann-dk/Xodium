using Sidekick.State;
using Xodium.Injection;
using Xodium.Mvvm;

namespace Sidekick.Uno
{
    public class UnoBootstrapper : AppBootstrapper
    {
        public UnoBootstrapper(StoreProvider<AppState> storeProvider = null)
            : base(storeProvider)
        {
        }

        protected override void RegisterServices(IDependencyRegistry registry)
        {
            base.RegisterServices(registry);

            //registry.RegisterFactory<INavigationService>(resolver => new NavigationService(App.NavigationPage, () => ViewRegistry));
            //registry.RegisterFactory<IDialogService>(resolver => new DialogService(App.NavigationPage, () => ViewRegistry));
        }

        protected override void RegisterViews(IViewRegistry registry)
        {
            base.RegisterViews(registry);

            //registry.RegisterViewType<ComponentLookupView, ComponentLookupViewModel>();
        }
    }
}
