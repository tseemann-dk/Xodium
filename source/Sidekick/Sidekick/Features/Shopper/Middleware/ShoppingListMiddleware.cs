using Redux;
using Sidekick.Features.Shopper.Models;
using Sidekick.State;

namespace Sidekick.Features.Shopper.Middleware
{
    public static class ShoppingListMiddleware
    {
        public static Middleware<AppState> CreateMiddleware()
        {
            return store => next => action =>
            {
                if (action is Actions.ComponentLookup.ComponentPickedAction a)
                {
                    var state = store.GetState();
                    var session = state.ShoppingSession;

                    // TODO: Get component from action

                    var componentNumber = state.Global.NextComponentNumber;
                    var component = new Component(ShopIdentity.Internal, componentNumber.ToString(), $"Component {componentNumber}", 10);
                    var item = new ShoppingItem(component, 1);

                    store.Dispatch(new Actions.ShoppingList.AddComponentAction(component));
                    store.Dispatch(new Actions.ShoppingList.AddItemAction(session.CurrentGroupId, item, insertAfterNodeId: session.FocusedNodeId));
                }

                return next(action);
            };
        }
    }
}
