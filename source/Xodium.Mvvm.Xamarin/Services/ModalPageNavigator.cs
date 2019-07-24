using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xodium.Mvvm.Xamarin.Services
{
    internal class ModalPageNavigator : IPageNavigator
    {
        private readonly INavigation navigation;

        public ModalPageNavigator(INavigation navigation)
        {
            this.navigation = navigation ?? throw new ArgumentNullException(nameof(navigation));
        }

        public IEnumerable<Page> Pages => navigation.ModalStack;
        public Page FirstPage => navigation.ModalStack.FirstOrDefault();
        public Page LastPage => navigation.ModalStack.LastOrDefault();
        public int PageCount => navigation.ModalStack.Count;
        public bool CanGoBack => navigation.ModalStack.Any();
        public bool IsAtRoot => PageCount == 1;
        public bool CanGoTo(Page page) => true;
        public Task GoTo(Page page) => navigation.PushModalAsync(page);
        public Task<Page> GoBack(bool animated) => navigation.PopModalAsync(animated);

        public async Task Reset()
        {
            while (Pages.Any())
            {
                await GoBack(false);
            }
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
                await navigation.PopModalAsync();
            }
        }
    }
}
