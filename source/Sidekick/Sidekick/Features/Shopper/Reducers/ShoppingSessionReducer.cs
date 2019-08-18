using System;
using System.Collections.Generic;
using Sidekick.Features.Shopper.Actions;
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

        private static AppState EnterGroup(AppState state, EnterGroupAction action) =>
            state.WithShoppingSession(state.ShoppingSession
                .WithCurrentGroupId(action.Payload.GroupId));

        private static AppState ExitGroup(AppState state)
        {
            var currentGroup = state.ShoppingSession.GetCurrentGroup();
            var parentGroupId = currentGroup?.GetParent(state.ShoppingSession.ShoppingList.Content)?.Id;

            if (parentGroupId == null) 
                return state;
            
            return state.WithShoppingSession(state.ShoppingSession
                .WithCurrentGroupId(parentGroupId, state.ShoppingSession.CurrentGroupId));
        }

        private static AppState FocusNode(AppState state, FocusNodeAction action) =>
            state.WithShoppingSession(state.ShoppingSession
                .WithFocusedNodeId(action.Payload.NodeId));
    }
}
