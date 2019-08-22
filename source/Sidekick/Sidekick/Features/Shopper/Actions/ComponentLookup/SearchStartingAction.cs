using Xodium.Redux;

namespace Sidekick.Features.Shopper.Actions.ComponentLookup
{
    public class SearchStartingAction : ReduxAction
    {
        public SearchStartingAction()
            : base(typeof(SearchStartingAction).FullName)
        {
        }
    }
}
