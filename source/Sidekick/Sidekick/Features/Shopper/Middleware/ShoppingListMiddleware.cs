using Sidekick.Features.Shopper.Actions;
using Sidekick.Features.Shopper.Actions.ComponentLookup;
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
                    ComponentPicked(store, a);
                }

                return next(action);
            };
        }

        private static void ComponentPicked(Redux.IStore<AppState> store, ComponentPickedAction action)
        {
            var state = store.GetState();
            var session = state.ShoppingSession;

            //var componentNumber = state.Global.NextComponentNumber;
            //var component = new Component(ShopIdentity.Internal, componentNumber.ToString(), $"Component {componentNumber}", 10);

            var c = action.Payload.Component;
            var component = state.ShoppingSession.ShoppingList.FindComponent(c.Reference);

            if (component == null)
            {
                component = new Component(c.Reference.Shop, c.Reference.ComponentNumber, c.Text, c.Price);
                store.Dispatch(ShoppingListActionCreator.AddComponent(component));
            }

            store.Dispatch(ShoppingListActionCreator.AddItem(
                session.CurrentGroupId,
                new ShoppingItem(component, 1), 
                insertAfterNodeId: session.FocusedNodeId
            ));
        }
    }
}
