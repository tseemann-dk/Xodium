using System;
using System.Collections.Generic;
using Sidekick.Features.Shopper.Actions;
using Sidekick.Features.Shopper.Models;
using Sidekick.State;
using Xodium.Productivity.Content.Models;

namespace Sidekick.Features.Shopper.Reducers
{
    public static class ShoppingListReducer
    {
        private static readonly Dictionary<Type, Func<AppState, object, AppState>> handlers = new Dictionary<Type, Func<AppState, object, AppState>>
        {
            [typeof(AddComponentAction)] = (s, a) => AddComponent(s, (AddComponentAction)a),
            [typeof(AddGroupAction)] = (s, a) => AddGroup(s, (AddGroupAction)a),
            [typeof(AddItemAction)] = (s, a) => AddItem(s, (AddItemAction)a),
            [typeof(ChangeGroupTitleAction)] = (s, a) => ChangeGroupTitle(s, (ChangeGroupTitleAction)a),
            [typeof(DeleteNodeAction)] = (s, a) => DeleteNode(s, (DeleteNodeAction)a),
            [typeof(MoveNodeDownAction)] = (s, a) => MoveNodeDown(s, (MoveNodeDownAction)a),
            [typeof(MoveNodeUpAction)] = (s, a) => MoveNodeUp(s, (MoveNodeUpAction)a),
        };

        public static AppState Execute(AppState state, object action) =>
            handlers.TryGetValue(action.GetType(), out var handler) ? handler.Invoke(state, action) : state;

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
