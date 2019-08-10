using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xodium.Platform.Xamarin.Services
{
    internal class PopupPageNavigator : IPageNavigator
    {
        private readonly IPopupNavigation navigation;
        private readonly Func<Page, Task> onPagePopped;

        public PopupPageNavigator(IPopupNavigation navigation, Func<Page, Task> onPagePopped)
        {
            this.navigation = navigation ?? throw new ArgumentNullException(nameof(navigation));
            this.onPagePopped = onPagePopped ?? throw new ArgumentNullException(nameof(onPagePopped));
        }

        public IEnumerable<Page> Pages => navigation.PopupStack;
        public Page FirstPage => navigation.PopupStack.FirstOrDefault();
        public Page LastPage => navigation.PopupStack.LastOrDefault();
        public int PageCount => navigation.PopupStack.Count();
        public bool CanGoBack => navigation.PopupStack.Any();
        public bool IsAtRoot => PageCount == 1;
        public bool CanGoTo(Page page) => page is PopupPage;
        public Task GoTo(Page page) => navigation.PushAsync(page as PopupPage);

        public async Task<Page> GoBack(bool animate)
        {
            var page = LastPage;
            await navigation.PopAsync(animate);
            await onPagePopped(page);
            return page;
        }

        public Task Reset()
        {
            return PageCount > 0 ? navigation.PopAllAsync() : Task.CompletedTask;
        }

        public async Task ResetTo(Page page)
        {
            await Reset();
            await GoTo(page);
        }

        public async Task ResetToRoot()
        {
            while (PageCount > 1)
            {
                await navigation.PopAsync();
            }
        }
    }
}
