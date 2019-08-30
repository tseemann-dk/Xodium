using Sidekick.Features.Shopper.Models;
using Xodium.Redux;

namespace Sidekick.Features.Shopper.Actions.ComponentLookup
{
    public class ComponentPickedAction : ReduxAction<ComponentPickedAction.Properties>
    {
        public ComponentPickedAction(IComponentDescriptor component)
            : base(typeof(HideLookupAction).FullName, new Properties(component))
        {
        }

        public struct Properties
        {
            public Properties(IComponentDescriptor component)
            {
                Component = component ?? throw new System.ArgumentNullException(nameof(component));
            }
            public IComponentDescriptor Component { get; }
        }
    }
}
