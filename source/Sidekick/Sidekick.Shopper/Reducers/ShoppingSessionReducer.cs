using System;
using System.Collections.Generic;
using Sidekick.Shopper.Actions.ShoppingSession;
using Sidekick.Shopper.Models;
using Sidekick.Shopper.State;
using Xodium.Flow;
using Xodium.Productivity.Content.Models;

namespace Sidekick.Shopper.Reducers
{
    public static class ShoppingSessionReducer
    {
        private static readonly Dictionary<Type, Reducer<ShoppingSession>> handlers = new Dictionary<Type, Reducer<ShoppingSession>>
        {
            [typeof(EnterGroupAction)] = (s, a) => EnterGroup(s, (EnterGroupAction)a),
            [typeof(ExitGroupAction)] = (s, _) => ExitGroup(s),
            [typeof(FocusNodeAction)] = (s, a) => FocusNode(s, (FocusNodeAction)a),
            [typeof(Actions.ShoppingList.AddGroupAction)] = (s, a) => AddGroup(s, (Actions.ShoppingList.AddGroupAction)a),
            [typeof(Actions.ShoppingList.AddItemAction)] = (s, a) => AddItem(s, (Actions.ShoppingList.AddItemAction)a),
            [typeof(Actions.ShoppingList.DeleteNodeAction)] = (s, a) => DeleteNode(s, (Actions.ShoppingList.DeleteNodeAction)a)
        };

        public static ShoppingSession Reduce(ShoppingSession state, object action) =>
            (handlers.TryGetValue(action.GetType(), out var handler) ? handler(state, action) : state)
                .WithComponentLookup(ComponentLookupReducer.Reduce(state.ComponentLookup, action))
                .WithShoppingList(ShoppingListReducer.Reduce(state.ShoppingList, action));

        private static ShoppingSession AddGroup(ShoppingSession state, Actions.ShoppingList.AddGroupAction action) =>
            state.WithFocusedNodeId(action.Payload.Group.Id);

        private static ShoppingSession AddItem(ShoppingSession state, Actions.ShoppingList.AddItemAction action) =>
            state.WithFocusedNodeId(action.Payload.Item.Id);

        private static ShoppingSession DeleteNode(ShoppingSession state, Actions.ShoppingList.DeleteNodeAction action)
        {
            var group = state.ShoppingList.FindGroup(action.Payload.ParentGroupId);
            var node = group.GetChildNode(action.Payload.NodeId) as IShoppingNode;
            var neighborId = (group.GetNextNode(node) ?? group.GetPreviousNode(node))?.Id;

            return state.WithFocusedNodeId(neighborId);
        }

        private static ShoppingSession EnterGroup(ShoppingSession state, EnterGroupAction action)
        {
            var nodeId = action.Payload.GroupId;
            var parentGroup = state.GetCurrentGroup();
            var node = parentGroup.GetChildNode(action.Payload.GroupId);

            if (node == null)
                throw new KeyNotFoundException($"Node {nodeId} was not found in group {parentGroup.Id}");

            if (!(node is IShoppingGroup))
                throw new InvalidCastException($"Node {nodeId} is not a group");

            return state.WithCurrentGroupId(nodeId);
        }

        private static ShoppingSession ExitGroup(ShoppingSession state)
        {
            var currentGroup = state.GetCurrentGroup();
            var parentGroupId = currentGroup?.GetParent(state.ShoppingList.Content)?.Id;

            if (parentGroupId == null) 
                return state;

            return state.WithCurrentGroupId(parentGroupId, state.CurrentGroupId);
        }

        private static ShoppingSession FocusNode(ShoppingSession state, FocusNodeAction action) =>
            state.WithFocusedNodeId(action.Payload.NodeId);
    }
}
