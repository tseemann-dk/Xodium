using Sidekick.Features.Shopper.Actions;
using Sidekick.Features.Shopper.Models;
using Xodium.Productivity.Content.Models;

namespace Sidekick.Features.Shopper.Reducers
{
    public class ShoppingSessionReducer
    {
        public static ShoppingSession Execute(ShoppingSession state, object action)
        {
            IShoppingList shoppingList = state.ShoppingList;
            IShoppingGroup newGroup = null;
            IComponent newComponent = null;

            var currentGroupId = state.CurrentGroupId;
            var focusedNodeId = state.FocusedNodeId;
            var currentGroup = state.ShoppingList.Content.FindNode<IShoppingGroup>(x => x.Id == state.CurrentGroupId);

            switch (action)
            {
                case EnterGroupAction a:
                    currentGroupId = a.Payload.GroupId;
                    focusedNodeId = null;
                    break;

                case ExitGroupAction a:
                    var parentGroupId = currentGroup?.GetParent(state.ShoppingList.Content)?.Id;
                    if (parentGroupId != null)
                    {
                        focusedNodeId = currentGroupId;
                        currentGroupId = parentGroupId;
                    }
                    break;

                case FocusNodeAction a:
                    focusedNodeId = a.Payload.NodeId;
                    break;

                case ChangeGroupTitleAction a:
                    newGroup = ShoppingGroupTransformer.ChangeTitle(
                        currentGroup, 
                        a.Payload.NewTitle);
                    break;

                case AddComponentAction a:
                    newComponent = a.Payload.Component;
                    break;

                case AddGroupAction a:
                    (newGroup, focusedNodeId) = ShoppingGroupTransformer.AddGroup(
                        currentGroup, 
                        a.Payload.GroupNumber, 
                        a.Payload.Text, 
                        a.Payload.Quantity, 
                        a.Payload.InsertAfterNodeId);
                    break;

                case AddShoppingItemAction a:
                    (newGroup, focusedNodeId) = ShoppingGroupTransformer.AddItem(
                        currentGroup, 
                        a.Payload.Component, 
                        a.Payload.Quantity, 
                        a.Payload.Text, 
                        a.Payload.Value, 
                        a.Payload.InsertAfterNodeId);
                    break;

                case DeleteNodeAction a:
                    (newGroup, focusedNodeId) = ShoppingGroupTransformer.DeleteNode(
                        currentGroup, 
                        a.Payload.NodeId);
                    break;

                case MoveNodeDownAction a:
                    (newGroup, focusedNodeId) = ShoppingGroupTransformer.MoveNodeDown(
                        currentGroup, 
                        a.Payload.NodeId);
                    break;

                case MoveNodeUpAction a:
                    (newGroup, focusedNodeId) = ShoppingGroupTransformer.MoveNodeUp(
                        currentGroup, 
                        a.Payload.NodeId);
                    break;
            }

            if (newGroup != null)
            {
                shoppingList = currentGroup.Id == shoppingList.Content.Id
                    ? state.ShoppingList.WithContent(newGroup) as ShoppingList
                    : state.ShoppingList.ReplaceNode(currentGroup, newGroup) as ShoppingList;
            }

            if (newComponent != null)
            {
                shoppingList = shoppingList.AddComponent(newComponent);
            }

            return new ShoppingSession
            {
                ShoppingList = shoppingList as ShoppingList,
                CurrentGroupId = currentGroupId,
                FocusedNodeId = focusedNodeId
            };
        }
    }
}
