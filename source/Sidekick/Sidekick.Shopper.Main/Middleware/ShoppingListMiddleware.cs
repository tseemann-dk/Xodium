using Sidekick.Shopper.Actions;
using Sidekick.Shopper.Models;
using Sidekick.Shopper.Actions.ComponentLookup;
using Sidekick.State;

namespace Sidekick.Shopper.Middleware
{
    public static class ShoppingListMiddleware
    {
        public static Redux.Middleware<AppState> CreateMiddleware()
        {
            return store => next => action =>
            {
                if (action is ComponentPickedAction a)
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

            var c = action.Payload.Component;
            var component = state.ShoppingSession.ShoppingList.FindComponent(c.Reference);

            if (component == null)
            {
                component = new Component(c.Reference.Shop, c.Reference.ComponentNumber, c.Text, c.ThumbnailUrl, c.Price);
                store.Dispatch(ShoppingListActionCreator.AddComponent(component));
            }

            store.Dispatch(ShoppingListActionCreator.AddItem(
                session.CurrentFolderId,
                new ShoppingItem(component, 1), 
                insertAfterNodeId: session.FocusedNodeId
            ));
        }
    }
}
