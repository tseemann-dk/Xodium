using Sidekick.Features.Shopper.Actions.ComponentLookup;
using Sidekick.Features.Shopper.Models;
using Sidekick.State;
using System;
using System.Collections.Generic;

namespace Sidekick.Features.Shopper.Reducers
{
    public static class ComponentLookupReducer
    {
        private static readonly Dictionary<Type, Func<AppState, object, AppState>> handlers = new Dictionary<Type, Func<AppState, object, AppState>>
        {
            [typeof(ChangeSearchTextAction)] = (s, a) => ChangeSearchText(s, (ChangeSearchTextAction)a),
            [typeof(ShowAction)] = (s, a) => ChangeIsVisible(s, true),
            [typeof(HideAction)] = (s, a) => ChangeIsVisible(s, false)
        };

        public static AppState Execute(AppState state, object action) =>
            handlers.TryGetValue(action.GetType(), out var handler) ? handler.Invoke(state, action) : state;

        private static AppState ChangeSearchText(AppState state, ChangeSearchTextAction action) =>
            UpdateComponentLookup(state, x => x
                .WithSearchText(action.Payload.NewSearchText));

        private static AppState ChangeIsVisible(AppState state, bool isVisible) =>
            UpdateComponentLookup(state, x => x
                .WithIsVisible(isVisible));

        private static AppState UpdateComponentLookup(AppState state, Func<ComponentLookup, ComponentLookup> update) =>
            state.WithShoppingSession(state.ShoppingSession
                .WithComponentLookup(update(state.ShoppingSession.ComponentLookup)));
    }
}
