using Sidekick.Features.Shopper.Reducers;
using System;

namespace Sidekick.State
{
    public class AppStateReducer
    {
        private static readonly Func<AppState, object, AppState>[] reducers =
        {
            GlobalStateReducer.Execute,
            ShoppingSessionReducer.Execute,
            ShoppingListReducer.Execute,
            ShopVisitReducer.Execute
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
