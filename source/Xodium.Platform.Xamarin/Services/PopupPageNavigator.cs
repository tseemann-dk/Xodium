using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xodium.Platform.Xamarin.Services
{
    internal class PopupPageNavigator : IPageNavigator
    {
        private readonly IPopupService popupService;
        private readonly Func<Page, Task> onPagePopped;

        public PopupPageNavigator(IPopupService popupService, Func<Page, Task> onPagePopped)
        {
            this.popupService = popupService ?? throw new ArgumentNullException(nameof(popupService));
            this.onPagePopped = onPagePopped ?? throw new ArgumentNullException(nameof(onPagePopped));
        }

        public IEnumerable<Page> Pages => popupService.PopupStack;
        public Page FirstPage => popupService.PopupStack.FirstOrDefault();
        public Page LastPage => popupService.PopupStack.LastOrDefault();
        public int PageCount => popupService.PopupStack.Count();
        public bool CanGoBack => popupService.PopupStack.Any();
        public bool IsAtRoot => PageCount == 1;
        public bool CanGoTo(Page page) => popupService.CanShowPage(page);
        public Task GoTo(Page page) => popupService.PushPage(page);

        public async Task<Page> GoBack(bool animate)
        {
            var page = LastPage;
            await popupService.PopPage(animate);
            await onPagePopped(page);
            return page;
        }

        public Task Reset()
        {
            return PageCount > 0 ? popupService.PopAllPages() : Task.CompletedTask;
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
                await popupService.PopPage();
            }
        }
    }
}
