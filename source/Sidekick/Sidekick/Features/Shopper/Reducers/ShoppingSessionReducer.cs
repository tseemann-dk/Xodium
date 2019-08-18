using Sidekick.Features.Shopper.Actions;
using Sidekick.Features.Shopper.Models;
using Sidekick.State;
using Xodium.Productivity.Content.Models;

namespace Sidekick.Features.Shopper.Reducers
{
    public class ShoppingSessionReducer
    {
        public static AppState Execute(AppState state, object action)
        {
            var session = state.ShoppingSession;
            var shoppingList = session.ShoppingList;
            var currentGroupId = session.CurrentGroupId;
            var currentGroup = shoppingList.Content.FindNode<IShoppingGroup>(x => x.Id == currentGroupId);

            AppState newShoppingSession(ShoppingSession s) => state.WithShoppingSession(s);

            switch (action)
            {
                case EnterGroupAction a:
                    return newShoppingSession(session.WithCurrentGroupId(a.Payload.GroupId));

                case ExitGroupAction a:
                    var parentGroupId = currentGroup?.GetParent(shoppingList.Content)?.Id;
                    if (parentGroupId == null) break;
                    return newShoppingSession(session.WithCurrentGroupId(parentGroupId, currentGroupId));

                case FocusNodeAction a:
                    return newShoppingSession(session.WithFocusedNodeId(a.Payload.NodeId));
            }

            return state;
        }
    }
}
