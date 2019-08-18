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
            switch (action)
            {
                case AddComponentAction a:
                    return AddComponent(state, a);

                case AddGroupAction a:
                    return AddGroup(state, a);

                case AddItemAction a:
                    return AddItem(state, a);

                case ChangeGroupTitleAction a:
                    return ChangeGroupTitle(state, a);

                case DeleteNodeAction a:
                    return DeleteNode(state, a);

                case MoveNodeDownAction a:
                    return MoveNodeDown(state, a);

                case MoveNodeUpAction a:
                    return MoveNodeUp(state, a);
            }

            return state;
        }

        private static AppState AddComponent(AppState state, AddComponentAction action) =>
            state.WithShoppingList(state.ShoppingSession.ShoppingList
                .AddComponent(action.Payload.Component));

        private static AppState AddGroup(AppState state, AddGroupAction action) =>
            state.WithShoppingSession(state.ShoppingSession
                .WithShoppingList(state.ShoppingSession.ShoppingList
                    .AddNode(state.ShoppingSession.GetCurrentGroup(), action.Payload.Group, action.Payload.InsertAfterNodeId))
                .WithFocusedNodeId(action.Payload.Group.Id));

        private static AppState AddItem(AppState state, AddItemAction action) =>
            state.WithShoppingSession(state.ShoppingSession
                .WithShoppingList(state.ShoppingSession.ShoppingList
                    .AddNode(state.ShoppingSession.GetCurrentGroup(), action.Payload.Item, action.Payload.InsertAfterNodeId))
                .WithFocusedNodeId(action.Payload.Item.Id));

        private static AppState ChangeGroupTitle(AppState state, ChangeGroupTitleAction action) => 
            state.WithShoppingList(state.ShoppingSession.ShoppingList
                .ChangeGroupTitle(state.ShoppingSession.GetCurrentGroup(), action.Payload.NewTitle));

        private static AppState DeleteNode(AppState state, DeleteNodeAction action)
        {
            var session = state.ShoppingSession;
            var currentGroup = session.GetCurrentGroup();
            var node = currentGroup.GetChildNode(action.Payload.NodeId) as IShoppingNode;
            var neighborId = (currentGroup.GetNextNode(node) ?? currentGroup.GetPreviousNode(node))?.Id;

            return state.WithShoppingSession(session
                .WithShoppingList(session.ShoppingList.DeleteNode(currentGroup, node))
                .WithFocusedNodeId(neighborId));
        }

        private static AppState MoveNodeDown(AppState state, MoveNodeDownAction action) =>
            state.WithShoppingList(state.ShoppingSession.ShoppingList
                .MoveNodeDown(state.ShoppingSession.GetCurrentGroup(), action.Payload.NodeId));

        private static AppState MoveNodeUp(AppState state, MoveNodeUpAction action) =>
            state.WithShoppingList(state.ShoppingSession.ShoppingList
                .MoveNodeUp(state.ShoppingSession.GetCurrentGroup(), action.Payload.NodeId));
    }
}
