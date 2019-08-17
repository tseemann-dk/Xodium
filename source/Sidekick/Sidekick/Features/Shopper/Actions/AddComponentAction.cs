using Sidekick.Features.Shopper.Models;
using Xodium.Redux;

namespace Sidekick.Features.Shopper.Actions
{
    public class AddComponentAction : ReduxAction<AddComponentAction.Properties>
    {
        public AddComponentAction(IComponent component)
            : base(typeof(AddComponentAction).FullName, new Properties(component))
        {
        }

        public struct Properties
        {
            public Properties(IComponent component)
            {
                Component = component;
            }

            public IComponent Component;
        }
    }
}
