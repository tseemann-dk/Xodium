using Xodium.Redux;

namespace Sidekick.Shopper.Actions.ComponentLookup
{
    public class SearchStartingAction : ReduxAction
    {
        public SearchStartingAction()
            : base(typeof(SearchStartingAction).FullName)
        {
        }
    }
}
