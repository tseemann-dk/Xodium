using Xodium.Redux;

namespace Sidekick.Shopper.Actions.ComponentLookup
{
    public class SetSearchTextAction : ReduxAction<SetSearchTextAction.Properties>
    {
        public SetSearchTextAction(string newSearchText)
            : base(typeof(SetSearchTextAction).FullName, new Properties(newSearchText))
        {
        }

        public struct Properties
        {
            public Properties(string newSearchText)
            {
                NewSearchText = newSearchText;
            }

            public string NewSearchText;
        }
    }
}
