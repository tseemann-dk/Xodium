using Xodium.Redux;

namespace Sidekick.Shopper.Actions.ComponentLookup
{
    public class PickComponentAction : ReduxAction
    {
        public PickComponentAction()
            : base(typeof(PickComponentAction).FullName)
        {
        }
    }
}
