using Xodium.Redux;

namespace Sidekick.Features.Shopper.Actions.ComponentLookup
{
    public class ShowAction : ReduxAction<ShowAction.Properties>
    {
        public ShowAction()
            : base(typeof(ShowAction).FullName, new Properties())
        {
        }

        public struct Properties
        {
        }
    }
}
