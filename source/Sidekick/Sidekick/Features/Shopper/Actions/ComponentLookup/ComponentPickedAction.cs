using Xodium.Redux;

namespace Sidekick.Features.Shopper.Actions.ComponentLookup
{
    public class ComponentPickedAction : ReduxAction<ComponentPickedAction.Properties>
    {
        public ComponentPickedAction()
            : base(typeof(HideAction).FullName, new Properties())
        {
        }

        public struct Properties
        {
        }
    }
}
