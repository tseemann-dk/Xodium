using Sidekick.Shopper.Actions.ShoppingList;
using Sidekick.Shopper.Models;
using Xodium.Flow;

namespace Sidekick.Shopper.Actions
{
    public static class ShoppingListActionCreator
    {
        public static IAction AddComponent(IComponent component) => new AddComponentAction(component);
        public static IAction AddGroup(string parentGroupId, IShoppingGroup group, string insertAfterNodeId = null) => new AddGroupAction(parentGroupId, group, insertAfterNodeId);
        public static IAction AddItem(string parentGroupId, IShoppingItem item, string insertAfterNodeId = null) => new AddItemAction(parentGroupId, item, insertAfterNodeId);
        public static IAction ChangeGroupTitle(string groupId, string newTitle) => new ChangeGroupTitleAction(groupId, newTitle);
        public static IAction DeleteNode(string parentGroupId, string nodeId) => new DeleteNodeAction(parentGroupId, nodeId);
        public static IAction MoveNodeDown(string parentGroupId, string nodeId) => new MoveNodeDownAction(parentGroupId, nodeId);
        public static IAction MoveNodeUp(string parentGroupId, string nodeId) => new MoveNodeUpAction(parentGroupId, nodeId);
    }
}
