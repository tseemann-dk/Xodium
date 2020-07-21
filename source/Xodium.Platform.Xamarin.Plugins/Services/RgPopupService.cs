using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Pages;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xodium.Platform.Xamarin.Services
{
    public class RgPopupService : IPopupService
    {
        private readonly IPopupNavigation popupNavigation;

        public RgPopupService(IPopupNavigation popupNavigation)
        {
            this.popupNavigation = popupNavigation;
        }

        public IReadOnlyCollection<Page> PopupStack => popupNavigation.PopupStack;

        public bool CanShowPage(Page page) => page is PopupPage;
        public Task PopAllPages(bool animate = true) => popupNavigation.PopAllAsync(animate);
        public Task PopPage(bool animate = true) => popupNavigation.PopAsync(animate);
        public Task PushPage(Page page, bool animate = true) => popupNavigation.PushAsync(page as PopupPage, animate);
    }
}
