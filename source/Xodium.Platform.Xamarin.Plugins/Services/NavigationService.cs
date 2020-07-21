using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xodium.Mvvm;

namespace Xodium.Platform.Xamarin.Services
{
    public class NavigationService : NavigationServiceBase
    {
        public NavigationService(NavigationPage page, IViewRegistryProvider viewRegistryProvider)
            : base(page?.Navigation, new RgPopupService(PopupNavigation.Instance), viewRegistryProvider)
        {
        }

        public NavigationService(INavigation navigation, IPopupService popupService, IViewRegistryProvider viewRegistryProvider)
            : base(navigation, popupService, viewRegistryProvider)
        {
        }

        protected override Page CreatePopupPage(View view, object viewModel)
        {
            return new PopupPage
            {
                BindingContext = viewModel,
                Content = CreatePopupView(view),
                CloseWhenBackgroundIsClicked = false
            };
        }
    }
}
