using Sidekick.Shopper.Actions.ShoppingSession;
using Xodium.Flow;

namespace Sidekick.Shopper.Actions
{
    public static class ShoppingSessionActionCreator
    {
        public static IAction EnterFolder(string folderId) => new EnterFolderAction(folderId);
        public static IAction ExitFolder() => new ExitFolderAction();
        public static IAction FocusNode(string nodeId) => new FocusNodeAction(nodeId);
    }
}
