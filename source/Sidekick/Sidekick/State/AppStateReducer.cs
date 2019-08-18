using System;
using Redux;
using Sidekick.Features.Shopper.Middleware;
using Sidekick.Features.Shopper.Reducers;

namespace Sidekick.State
{
    public class AppStateReducer
    {
        private static readonly Reducer<AppState>[] reducers =
        {
            GlobalStateReducer.Execute,
            ShoppingSessionReducer.Execute,
            ShoppingListReducer.Execute,
            ComponentLookupReducer.Execute
        };

        public static Middleware<AppState>[] Middlewares { get; } =
        {
            ComponentLookupMiddleware.Execute
        };

        public static AppState Execute(AppState state, object action)
        {
            foreach (var reducer in reducers)
            {
                state = reducer(state, action);
            }

            return state;
        }
    }
}
