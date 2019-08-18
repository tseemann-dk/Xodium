using Xodium.Redux;

namespace Sidekick.Features.Shopper.Actions.ShoppingSession
{
    public class ExitGroupAction : ReduxAction<ExitGroupAction.Properties>
    {
        public ExitGroupAction()
            : base(typeof(ExitGroupAction).FullName, new Properties())
        {
        }

        public struct Properties
        {
        }
    }
}
