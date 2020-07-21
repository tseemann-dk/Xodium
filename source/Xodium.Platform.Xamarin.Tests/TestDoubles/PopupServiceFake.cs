using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xodium.Platform.Xamarin.Services;

namespace Xodium.Platform.Xamarin.Tests.TestDoubles
{
    public class PopupServiceFake : IPopupService
    {
        private readonly Stack<Page> pageStack;

        public PopupServiceFake()
        {
            pageStack = new Stack<Page>();
        }

        public IReadOnlyCollection<Page> PopupStack => pageStack;

        public bool CanShowPage(Page page) => true;

        public Task PopAllPages(bool animate = true)
        {
            pageStack.Clear();
            return Task.CompletedTask;
        }

        public Task PopPage(bool animate = true)
        {
            pageStack.Pop();
            return Task.CompletedTask;
        }

        public Task PushPage(Page page, bool animate = true)
        {
            pageStack.Push(page);
            return Task.CompletedTask;
        }
    }
}
