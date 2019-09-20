using Sidekick.Shopper.Models;
using Xodium.Redux;

namespace Sidekick.Shopper.Actions.ShoppingList
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
