using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xodium.Mvvm;

namespace Xodium.Platform.Xamarin.Services
{
    public class CompositePageNavigator : IPageNavigator
    {
        private readonly IEnumerable<IPageNavigator> navigators;

        public CompositePageNavigator(IEnumerable<IPageNavigator> navigators)
        {
            this.navigators = navigators?.ToList() ?? throw new ArgumentNullException(nameof(navigators));
        }

        public IPageNavigator CurrentNavigator => navigators.LastOrDefault(x => x.PageCount > 0);

        public IEnumerable<Page> Pages => navigators.SelectMany(x => x.Pages);
        public Page FirstPage => CurrentNavigator?.FirstPage;
        public Page LastPage => CurrentNavigator?.LastPage;
        public int PageCount => navigators.Sum(x => x.PageCount);
        public bool CanGoBack => PageCount > 0;
        public bool IsAtRoot => PageCount == 1;

        public bool CanGoTo(Page page) => navigators.Any(x => x.CanGoTo(page));
        public Task GoTo(Page page) => navigators.First()?.GoTo(page) ?? Task.CompletedTask;
        public Task<Page> GoBack(bool animated) => CurrentNavigator?.GoBack(animated) ?? throw new NavigationException("Cannot navigate back");

        public async Task Reset()
        {
            foreach (var navigator in navigators.Reverse())
            {
                await navigator.Reset();
            }
        }

        public async Task ResetTo(Page page)
        {
            foreach (var navigator in navigators.Skip(1).Reverse())
            {
                await navigator.Reset();
            }

            await navigators.First().ResetTo(page);
        }

        public async Task ResetToRoot()
        {
            while (PageCount > 1)
            {
                await GoBack(false);
            }
        }
    }
}
