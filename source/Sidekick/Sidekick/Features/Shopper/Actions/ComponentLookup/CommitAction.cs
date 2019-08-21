using Xodium.Redux;

namespace Sidekick.Features.Shopper.Actions.ComponentLookup
{
    public class CommitAction : ReduxAction<CommitAction.Properties>
    {
        public CommitAction()
            : base(typeof(CommitAction).FullName, new Properties())
        {
        }

        public struct Properties
        {
        }
    }
}
