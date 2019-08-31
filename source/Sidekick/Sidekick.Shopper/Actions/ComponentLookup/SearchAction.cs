using Xodium.Redux;

namespace Sidekick.Shopper.Actions.ComponentLookup
{
    public class SearchAction : ReduxAction<SearchAction.Properties>
    {
        public SearchAction(string searchText)
            : base(typeof(SearchAction).FullName, new Properties(searchText))
        {
        }

        public class Properties
        {
            public Properties(string searchText)
            {
                SearchText = searchText;
            }

            public string SearchText { get; }
        }
    }
}
