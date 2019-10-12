using Sidekick.Shopper.Actions.ShoppingList;
using Sidekick.Shopper.Models;
using Xodium.Flow;

namespace Sidekick.Shopper.Actions
{
    public static class ShoppingListActionCreator
    {
        public static IAction AddComponent(IComponent component) => new AddComponentAction(component);
        public static IAction AddFolder(string parentFolderId, IShoppingFolder folder, string insertAfterNodeId = null) => new AddFolderAction(parentFolderId, folder, insertAfterNodeId);
        public static IAction AddItem(string parentFolderId, IShoppingItem item, string insertAfterNodeId = null) => new AddItemAction(parentFolderId, item, insertAfterNodeId);
        public static IAction ChangeFolderTitle(string folderId, string newTitle) => new ChangeFolderTitleAction(folderId, newTitle);
        public static IAction DeleteNode(string folderId, string nodeId) => new DeleteNodeAction(folderId, nodeId);
        public static IAction MoveNodeDown(string folderId, string nodeId) => new MoveNodeDownAction(folderId, nodeId);
        public static IAction MoveNodeUp(string folderId, string nodeId) => new MoveNodeUpAction(folderId, nodeId);
    }
}
