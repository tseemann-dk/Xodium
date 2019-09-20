using Sidekick.Shopper.Actions.ShoppingSession;
using Xodium.Flow;

namespace Sidekick.Shopper.Actions
{
    public static class ShoppingSessionActionCreator
    {
        public static IAction EnterGroup(string groupId) => new EnterGroupAction(groupId);
        public static IAction ExitGroup() => new ExitGroupAction();
        public static IAction FocusNode(string nodeId) => new FocusNodeAction(nodeId);
    }
}
