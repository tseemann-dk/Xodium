using Redux;
using Sidekick.State;

namespace Sidekick.Features.Shopper.Middleware
{
    public static class ComponentLookupMiddleware
    {
        public static Middleware<AppState> CreateMiddleware()
        {
            return store => next => action =>
            {
                if (action is Actions.ComponentLookup.CommitAction)
                {
                    var state = store.GetState();

                    store.Dispatch(new Actions.ComponentLookup.ComponentPickedAction(state.ShoppingSession.ComponentLookup.GetSelectedComponent()));
                    store.Dispatch(new Actions.ComponentLookup.HideAction());
                }

                return next(action);
            };
        }
    }
}
