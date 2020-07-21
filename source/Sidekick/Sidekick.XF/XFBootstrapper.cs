using Sidekick.Shopper.UI.XF.Views;
using Sidekick.Shopper.ViewModels;
using Sidekick.State;
using Xodium.Injection;
using Xodium.Mvvm;
using Xodium.Platform.Xamarin.Services;

namespace Sidekick.XF
{
    public class XFBootstrapper : AppBootstrapper
    {
        public XFBootstrapper(StoreProvider<AppState> storeProvider = null)
            : base(storeProvider)
        {
        }

        protected override void RegisterServices(IDependencyRegistry registry)
        {
            base.RegisterServices(registry);

            registry.RegisterFactory<INavigationService>(resolver => new NavigationService(App.NavigationPage, new ViewRegistryProvider(() => ViewRegistry)));
            registry.RegisterFactory<IDialogService>(resolver => new DialogService(App.NavigationPage, () => ViewRegistry));
        }

        protected override void RegisterViews(IViewRegistry registry)
        {
            base.RegisterViews(registry);

            registry.RegisterViewType<ComponentLookupView, ComponentLookupViewModel>();
        }
    }
}
