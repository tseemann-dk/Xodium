using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xodium.Platform.Xamarin.Tests.Utilities;

namespace Xodium.Platform.Xamarin.Tests.TestDoubles
{
    public class NavigationFake : INavigation
    {
        private PageStack<Page> modalStack = new PageStack<Page>();
        private PageStack<Page> regularStack = new PageStack<Page>();

        public IReadOnlyList<Page> ModalStack => modalStack.Pages;
        public IReadOnlyList<Page> NavigationStack => regularStack.Pages;

        public event EventHandler<PageEventArgs> PagePopped;

        public void InsertPageBefore(Page page, Page before) => regularStack.InsertBefore(page, before);
        public Task<Page> PopAsync() => PopPage(regularStack);
        public Task<Page> PopAsync(bool animated) => PopAsync();
        public Task<Page> PopModalAsync() => PopPage(modalStack);
        public Task<Page> PopModalAsync(bool animated) => PopModalAsync();
        public Task PopToRootAsync() => PopToRoot(regularStack);
        public Task PopToRootAsync(bool animated) => PopToRootAsync();
        public Task PushAsync(Page page) => regularStack.Push(page);
        public Task PushAsync(Page page, bool animated) => PushAsync(page);
        public Task PushModalAsync(Page page) => modalStack.Push(page);
        public Task PushModalAsync(Page page, bool animated) => PushModalAsync(page);
        public void RemovePage(Page page) => regularStack.RemovePage(page);

        private async Task<Page> PopPage(PageStack<Page> stack)
        {
            var page = await stack.Pop();
            OnPagePopped(page);
            return page;
        }

        private async Task PopToRoot(PageStack<Page> stack)
        {
            while (stack.Pages.Count > 1)
            {
                await PopPage(stack);
            }
        }

        private void OnPagePopped(Page page)
        {
            PagePopped?.Invoke(this, new PageEventArgs(page));
        }
    }

    public class PageEventArgs : EventArgs
    {
        public PageEventArgs(Page page)
        {
            Page = page ?? throw new ArgumentNullException(nameof(page));
        }

        public Page Page { get; }
    }
}
