using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Events;
using Rg.Plugins.Popup.Pages;
using Xodium.Platform.Xamarin.Tests.Utilities;

namespace Xodium.Platform.Xamarin.Tests.TestDoubles
{
    public class PopupNavigationFake : IPopupNavigation
    {
        private PageStack<PopupPage> stack = new PageStack<PopupPage>();
        public IReadOnlyList<PopupPage> PopupStack => stack.Pages;

        public event EventHandler<PopupNavigationEventArgs> Pushing;
        public event EventHandler<PopupNavigationEventArgs> Pushed;
        public event EventHandler<PopupNavigationEventArgs> Popping;
        public event EventHandler<PopupNavigationEventArgs> Popped;

        public async Task PopAllAsync(bool animate = true)
        {
            while (stack.Pages.Count > 0)
            {
                await PopAsync(animate);
            }
        }

        public Task PopAsync(bool animate = true) => stack.Pop();
        public Task PushAsync(PopupPage page, bool animate = true) => stack.Push(page);
        public Task RemovePageAsync(PopupPage page, bool animate = true) => stack.RemovePage(page);
    }
}
