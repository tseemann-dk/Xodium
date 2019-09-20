using Xodium.Redux;

namespace Sidekick.Shopper.Actions.ComponentLookup
{
    public class SelectComponentAction : ReduxAction<SelectComponentAction.Properties>
    {
        public SelectComponentAction(string componentNumber)
            : base(typeof(SelectComponentAction).FullName, new Properties(componentNumber))
        {
        }

        public struct Properties
        {
            public Properties(string componentNumber)
            {
                ComponentNumber = componentNumber;
            }

            public string ComponentNumber;
        }
    }
}
