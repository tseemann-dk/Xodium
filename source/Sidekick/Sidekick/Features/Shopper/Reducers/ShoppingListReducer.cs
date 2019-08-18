using Sidekick.Features.Shopper.Actions;
using Sidekick.Features.Shopper.Models;
using Sidekick.State;
using Xodium.Productivity.Content.Models;

namespace Sidekick.Features.Shopper.Reducers
{
    public class ShoppingListReducer
    {
        public static AppState Execute(AppState state, object action)
        {
            var session = state.ShoppingSession;
            var shoppingList = session.ShoppingList;
            var currentGroupId = session.CurrentGroupId;
            var currentGroup = shoppingList.Content.FindNode<IShoppingGroup>(x => x.Id == currentGroupId);

            AppState newShoppingSession(ShoppingSession s) => state.WithShoppingSession(s);
            AppState newShoppingList(ShoppingList l) => newShoppingSession(session.WithShoppingList(l));

            switch (action)
            {
                case ChangeGroupTitleAction a:
                    return newShoppingList(shoppingList.ChangeGroupTitle(currentGroup, a.Payload.NewTitle));

                case AddComponentAction a:
                    return newShoppingList(shoppingList.AddComponent(a.Payload.Component));

                case AddGroupAction a:
                    return newShoppingSession(session
                        .WithShoppingList(shoppingList.AddNode(currentGroup, a.Payload.Group, a.Payload.InsertAfterNodeId))
                        .WithFocusedNodeId(a.Payload.Group.Id)
                    );

                case AddShoppingItemAction a:
                    return newShoppingSession(session
                        .WithShoppingList(shoppingList.AddNode(currentGroup, a.Payload.Item, a.Payload.InsertAfterNodeId))
                        .WithFocusedNodeId(a.Payload.Item.Id)
                    );

                case DeleteNodeAction a:
                    var node = currentGroup.GetChildNode(a.Payload.NodeId) as IShoppingNode;
                    var neighborId = (currentGroup.GetNextNode(node) ?? currentGroup.GetPreviousNode(node))?.Id;
                    return newShoppingSession(session
                        .WithShoppingList(shoppingList.DeleteNode(currentGroup, node))
                        .WithFocusedNodeId(neighborId)
                    );

                case MoveNodeDownAction a:
                    return newShoppingList(shoppingList.MoveNodeDown(currentGroup, a.Payload.NodeId));

                case MoveNodeUpAction a:
                    return newShoppingList(shoppingList.MoveNodeUp(currentGroup, a.Payload.NodeId));
            }

            return state;
        }
    }
}
