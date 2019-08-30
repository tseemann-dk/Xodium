using Sidekick.Features.Shopper.Models;
using Sidekick.State;

namespace Sidekick.Features.Shopper.Middleware
{
    public static class ShoppingListMiddleware
    {
        public static Redux.Middleware<AppState> CreateMiddleware()
        {
            return store => next => action =>
            {
                if (action is Actions.ComponentLookup.ComponentPickedAction a)
                {
                    var state = store.GetState();
                    var session = state.ShoppingSession;

                    //var componentNumber = state.Global.NextComponentNumber;
                    //var component = new Component(ShopIdentity.Internal, componentNumber.ToString(), $"Component {componentNumber}", 10);

                    var c = a.Payload.Component;
                    var component = new Component(c.Reference.Shop, c.Reference.ComponentNumber, c.Text, c.Price);
                    var item = new ShoppingItem(component, 1);

                    store.Dispatch(new Actions.ShoppingList.AddComponentAction(component));
                    store.Dispatch(new Actions.ShoppingList.AddItemAction(session.CurrentGroupId, item, insertAfterNodeId: session.FocusedNodeId));
                }

                return next(action);
            };
        }
    }
}
