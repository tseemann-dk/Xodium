using Xodium.Redux;

namespace Sidekick.Features.Shopper.Actions
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
