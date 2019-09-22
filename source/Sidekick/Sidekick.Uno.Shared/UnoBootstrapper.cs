using Sidekick.State;
using Xodium.Injection;
using Xodium.Mvvm;
//using Xodium.Platform.Uno.Services;

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

            //var ds = new DialogService(() => ViewRegistry);

            //registry.RegisterFactory<IDialogService>(resolver => ds);
            //registry.RegisterFactory<INavigationService>(resolver => new NavigationService(() => App.Current.NavigationFrame, () => ViewRegistry, ds));
        }

        protected override void RegisterViews(IViewRegistry registry)
        {
            base.RegisterViews(registry);

            //registry.RegisterViewType<ComponentLookupView, ComponentLookupViewModel>();
        }
    }
}
