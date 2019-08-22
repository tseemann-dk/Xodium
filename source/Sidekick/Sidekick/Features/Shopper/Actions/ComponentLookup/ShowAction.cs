using Xodium.Redux;

namespace Sidekick.Features.Shopper.Actions.ComponentLookup
{
    public class ShowAction : ReduxAction
    {
        public ShowAction()
            : base(typeof(ShowAction).FullName)
        {
        }
    }
}
