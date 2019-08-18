using Sidekick.Features.Shopper.Actions;
using Sidekick.State;
using Xodium.Productivity.Content.Models;

namespace Sidekick.Features.Shopper.Reducers
{
    public class ShoppingSessionReducer
    {
        public static AppState Execute(AppState state, object action)
        {
            switch (action)
            {
                case EnterGroupAction a:
                    return EnterGroup(state, a);

                case ExitGroupAction _:
                    return ExitGroup(state);

                case FocusNodeAction a:
                    return FocusNode(state, a);
            }

            return state;
        }

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
