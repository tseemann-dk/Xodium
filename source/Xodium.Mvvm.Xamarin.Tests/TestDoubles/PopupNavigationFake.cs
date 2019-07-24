using System.Collections.Generic;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Pages;
using Xodium.Mvvm.Xamarin.Test.Utilities;

namespace Xodium.Mvvm.Xamarin.Test.TestDoubles
{
    public class PopupNavigationFake : IPopupNavigation
    {
        private PageStack<PopupPage> stack = new PageStack<PopupPage>();
        public IReadOnlyList<PopupPage> PopupStack => stack.Pages;

        public Task PopAllAsync(bool animate = true) => stack.PopAll();
        public Task PopAsync(bool animate = true) => stack.Pop();
        public Task PushAsync(PopupPage page, bool animate = true) => stack.Push(page);
        public Task RemovePageAsync(PopupPage page, bool animate = true) => stack.RemovePage(page);
    }
}
