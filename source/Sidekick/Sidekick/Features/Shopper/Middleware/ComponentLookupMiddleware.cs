using System;
using Sidekick.Features.Shopper.Actions;
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
                if (action is Actions.ComponentLookup.CommitAction)
                {
                    var state = store.GetState();

                    store.Dispatch(ComponentLookupActionCreator.ComponentPicked(state.ShoppingSession.ComponentLookup.GetSelectedComponent()));
                    store.Dispatch(ComponentLookupActionCreator.Hide());
                }

                if (action is Actions.ComponentLookup.SearchAction a)
                {
                    store.Dispatch(ComponentLookupActionCreator.SearchStarting());
                    try
                    {
                        //await Task.Delay(2000);

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

                return next(action);
            };
        }
    }
}
