using System;
using System.Threading.Tasks;
using Sidekick.Shopper.Actions;
using Sidekick.Shopper.Actions.ComponentLookup;
using Sidekick.Shopper.Models;
using Sidekick.State;

namespace Sidekick.Shopper.Middleware
{
    public static class ComponentLookupMiddleware
    {
        public static Redux.Middleware<AppState> CreateMiddleware()
        {
            return store => next => action =>
            {
                if (action is SearchAction a)
                {
                    return Search(store, a);
                }

                if (action is PickComponentAction)
                {
                    PickComponent(store);
                }

                return next(action);
            };
        }

        private static void PickComponent(Redux.IStore<AppState> store)
        {
            var component = store.GetState().ShoppingSession.ComponentLookup.GetSelectedComponent();

            if (component != null)
            {
                store.Dispatch(ComponentLookupActionCreator.ComponentPicked(component));
                store.Dispatch(ComponentLookupActionCreator.HideLookup());
            }
        }

        private static async Task Search(Redux.IStore<AppState> store, SearchAction action)
        {
            store.Dispatch(ComponentLookupActionCreator.SearchStarting());
            try
            {
                var shop = action.Payload.Shop;
                var searchText = action.Payload.SearchText;

                if (shop == null)
                    throw new InvalidOperationException("Shop is missing");

                if (searchText.Length <= 3)
                    throw new InvalidOperationException($"Search text \"{searchText}\" is too short");

                await Task.Delay(2000);

                var components = await shop.FindComponents(searchText);
                store.Dispatch(ComponentLookupActionCreator.SearchCompleted(components));

                //store.Dispatch(ComponentLookupActionCreator.SearchCompleted(new[]
                //{
                //    new ComponentDescriptor(new ComponentReference(ShopIdentity.Internal, "C001"), "First Component", 10),
                //    new ComponentDescriptor(new ComponentReference(ShopIdentity.Internal, "C002"), "Second Component", 20),
                //    new ComponentDescriptor(new ComponentReference(ShopIdentity.Internal, "C003"), "Third Component", 30)
                //}));
            }
            catch (Exception exception)
            {
                store.Dispatch(ComponentLookupActionCreator.SearchFailed(exception));
            }
        }
    }
}
