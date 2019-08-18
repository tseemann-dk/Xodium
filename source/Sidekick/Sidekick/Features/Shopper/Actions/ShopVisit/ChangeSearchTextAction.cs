using Xodium.Redux;

namespace Sidekick.Features.Shopper.Actions.ShopVisit
{
    public class ChangeSearchTextAction : ReduxAction<ChangeSearchTextAction.Properties>
    {
        public ChangeSearchTextAction(string newSearchText)
            : base(typeof(ChangeSearchTextAction).FullName, new Properties(newSearchText))
        {
        }

        public struct Properties
        {
            public Properties(string newSearchText)
            {
                NewSearchText = newSearchText;
            }

            public string NewSearchText;
        }
    }
}
