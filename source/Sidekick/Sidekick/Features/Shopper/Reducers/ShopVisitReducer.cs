using Sidekick.Features.Shopper.Actions.ShopVisit;
using Sidekick.State;
using System;
using System.Collections.Generic;

namespace Sidekick.Features.Shopper.Reducers
{
    public static class ShopVisitReducer
    {
        private static readonly Dictionary<Type, Func<AppState, object, AppState>> handlers = new Dictionary<Type, Func<AppState, object, AppState>>
        {
            [typeof(ChangeSearchTextAction)] = (s, a) => ChangeSearchText(s, (ChangeSearchTextAction)a)
        };

        public static AppState Execute(AppState state, object action) =>
            handlers.TryGetValue(action.GetType(), out var handler) ? handler.Invoke(state, action) : state;

        private static AppState ChangeSearchText(AppState state, ChangeSearchTextAction action) =>
            state.WithShoppingSession(state.ShoppingSession
                .WithShopVisit(state.ShoppingSession.ShopVisit
                    .WithSearchText(action.Payload.NewSearchText)));
    }
}
