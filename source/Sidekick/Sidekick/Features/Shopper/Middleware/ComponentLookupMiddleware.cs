using System;
using System.Threading.Tasks;
using Sidekick.Features.Shopper.Actions;
using Sidekick.Features.Shopper.Actions.ComponentLookup;
using Sidekick.Features.Shopper.Models;
using Sidekick.State;

namespace Sidekick.Features.Shopper.Middleware
{
    public static class ComponentLookupMiddleware
    {
        public static Redux.Middleware<AppState> CreateMiddleware()
        {
            return store => next => action =>
            {
                if (action is PickComponentAction)
                {
                    PickComponent(store);
                }

                if (action is SearchAction a)
                {
                    return Search(store, a);
                }

                return next(action);
            };
        }

        private static void PickComponent(Redux.IStore<AppState> store)
        {
            var state = store.GetState();

            store.Dispatch(ComponentLookupActionCreator.ComponentPicked(state.ShoppingSession.ComponentLookup.GetSelectedComponent()));
            store.Dispatch(ComponentLookupActionCreator.Hide());
        }

        private static async Task Search(Redux.IStore<AppState> store, SearchAction action)
        {
            store.Dispatch(ComponentLookupActionCreator.SearchStarting());
            try
            {
                // Search for action.Payload.SearchText
                await Task.Delay(2000);

                //throw new InvalidOperationException("Not supported");

                store.Dispatch(ComponentLookupActionCreator.SearchCompleted(new[]
                {
                    new ComponentDescriptor(new ComponentReference(ShopIdentity.Internal, "C001"), "First Component", 10),
                    new ComponentDescriptor(new ComponentReference(ShopIdentity.Internal, "C002"), "Second Component", 20),
                    new ComponentDescriptor(new ComponentReference(ShopIdentity.Internal, "C003"), "Third Component", 30)
                }));
            }
            catch (Exception exception)
            {
                store.Dispatch(ComponentLookupActionCreator.SearchFailed(exception));
            }
        }
    }
}
