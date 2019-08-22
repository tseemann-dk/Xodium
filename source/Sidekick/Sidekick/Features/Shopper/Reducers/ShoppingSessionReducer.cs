using System;
using System.Collections.Generic;
using Sidekick.Features.Shopper.Actions.ShoppingSession;
using Sidekick.Features.Shopper.Models;
using Sidekick.State;
using Xodium.Productivity.Content.Models;

namespace Sidekick.Features.Shopper.Reducers
{
    public static class ShoppingSessionReducer
    {
        private static readonly Dictionary<Type, Func<AppState, object, AppState>> handlers = new Dictionary<Type, Func<AppState, object, AppState>>
        {
            [typeof(EnterGroupAction)] = (s, a) => EnterGroup(s, (EnterGroupAction)a),
            [typeof(ExitGroupAction)] = (s, _) => ExitGroup(s),
            [typeof(FocusNodeAction)] = (s, a) => FocusNode(s, (FocusNodeAction)a),
        };

        public static AppState Execute(AppState state, object action) => 
            handlers.TryGetValue(action.GetType(), out var handler) ? handler.Invoke(state, action) : state;

        private static AppState EnterGroup(AppState state, EnterGroupAction action)
        {
            var nodeId = action.Payload.GroupId;
            var parentGroup = state.ShoppingSession.GetCurrentGroup();
            var node = parentGroup.GetChildNode(action.Payload.GroupId);

            if (node == null)
                throw new KeyNotFoundException($"Node {nodeId} was not found in group {parentGroup.Id}");

            if (!(node is IShoppingGroup))
                throw new InvalidCastException($"Node {nodeId} is not a group");

            return ReduceShoppingSession(state, x => x
                .WithCurrentGroupId(nodeId));
        }

        private static AppState ExitGroup(AppState state)
        {
            var currentGroup = state.ShoppingSession.GetCurrentGroup();
            var parentGroupId = currentGroup?.GetParent(state.ShoppingSession.ShoppingList.Content)?.Id;

            if (parentGroupId == null) 
                return state;

            return ReduceShoppingSession(state, x => x
                .WithCurrentGroupId(parentGroupId, state.ShoppingSession.CurrentGroupId));
        }

        private static AppState FocusNode(AppState state, FocusNodeAction action) =>
            ReduceShoppingSession(state, x => x
                .WithFocusedNodeId(action.Payload.NodeId));

        private static AppState ReduceShoppingSession(AppState state, Func<ShoppingSession, ShoppingSession> reduce) =>
            state.WithShoppingSession(reduce(state.ShoppingSession));
    }
}
