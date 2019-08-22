using Xodium.Redux;

namespace Sidekick.Features.Shopper.Actions.ComponentLookup
{
    public class HideAction : ReduxAction
    {
        public HideAction()
            : base(typeof(HideAction).FullName)
        {
        }
    }
}
