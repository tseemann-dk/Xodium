using Xodium.Redux;

namespace Sidekick.Features.Shopper.Actions.ComponentLookup
{
    public class HideAction : ReduxAction<HideAction.Properties>
    {
        public HideAction()
            : base(typeof(HideAction).FullName, new Properties())
        {
        }

        public struct Properties
        {
        }
    }
}
