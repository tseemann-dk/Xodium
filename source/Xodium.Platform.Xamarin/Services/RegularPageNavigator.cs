using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xodium.Platform.Xamarin.Services
{
    internal class RegularPageNavigator : IPageNavigator
    {
        private readonly INavigation navigation;

        public RegularPageNavigator(INavigation navigation)
        {
            this.navigation = navigation ?? throw new ArgumentNullException(nameof(navigation));
        }

        public IEnumerable<Page> Pages => navigation.NavigationStack;
        public Page FirstPage => navigation.NavigationStack.FirstOrDefault();
        public Page LastPage => navigation.NavigationStack.LastOrDefault();
        public int PageCount => navigation.NavigationStack.Count;
        public bool CanGoBack => navigation.NavigationStack.Any();
        public bool IsAtRoot => PageCount == 1;

        public bool CanGoTo(Page page) => true;
        public Task GoTo(Page page) => navigation.PushAsync(page);
        public Task<Page> GoBack(bool animated) => navigation.PopAsync(animated);

        public async Task Reset()
        {
            await navigation.PopToRootAsync(false);
            await navigation.PopAsync(false);
        }

        public async Task ResetTo(Page page)
        {
            if (Pages.Any())
            {
                navigation.InsertPageBefore(page, Pages.First());
                await navigation.PopToRootAsync(false);
            }
            else
            {
                await GoTo(page);
            }
        }

        public Task ResetToRoot()
        {
            return navigation.PopToRootAsync(false);
        }
    }
}
