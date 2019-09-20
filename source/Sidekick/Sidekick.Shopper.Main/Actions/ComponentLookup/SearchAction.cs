using Sidekick.Shopper.Models;
using Xodium.Redux;

namespace Sidekick.Shopper.Actions.ComponentLookup
{
    public class SearchAction : ReduxAction<SearchAction.Properties>
    {
        public SearchAction(string searchText, IShop shop)
            : base(typeof(SearchAction).FullName, new Properties(searchText, shop))
        {
        }

        public class Properties
        {
            public Properties(string searchText, IShop shop)
            {
                SearchText = searchText;
                Shop = shop;
            }

            public string SearchText { get; }
            public IShop Shop { get; }
        }
    }
}
