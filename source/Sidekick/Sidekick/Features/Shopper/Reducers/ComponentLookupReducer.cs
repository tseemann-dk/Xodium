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
            [typeof(SearchCompletedAction)] = (s, a) => SearchCompleted(s, (SearchCompletedAction)a),
            [typeof(SearchFailedAction)] = (s, a) => SearchFailed(s, (SearchFailedAction)a),
            [typeof(SearchStartingAction)] = (s, _) => SearchStarting(s),
            [typeof(SelectComponentAction)] = (s, a) => SelectComponent(s, (SelectComponentAction)a),
            [typeof(ShowAction)] = (s, a) => ChangeIsVisible(s, true),
            [typeof(HideAction)] = (s, a) => ChangeIsVisible(s, false)
        };

        public static AppState Execute(AppState state, object action) =>
            handlers.TryGetValue(action.GetType(), out var handler) ? handler.Invoke(state, action) : state;

        private static AppState ChangeSearchText(AppState state, ChangeSearchTextAction action) =>
            ReduceComponentLookup(state, x => x
                .WithSearchText(action.Payload.NewSearchText));

        private static AppState ChangeIsVisible(AppState state, bool isVisible) =>
            ReduceComponentLookup(state, x => x
                .WithIsVisible(isVisible));

        private static AppState SearchCompleted(AppState state, SearchCompletedAction action) =>
            ReduceComponentLookup(state, x => x
                .WithFoundComponents(action.Payload.Result)
                .WithSearchError(null)
                .WithIsSearching(false));

        private static AppState SearchFailed(AppState state, SearchFailedAction action) =>
            ReduceComponentLookup(state, x => x
                .WithFoundComponents(null)
                .WithSearchError(action.Payload.Exception.Message)
                .WithIsSearching(false));

        private static AppState SearchStarting(AppState state) =>
            ReduceComponentLookup(state, x => x
                .WithSearchError(null)
                .WithFoundComponents(null)
                .WithIsSearching(true));

        private static AppState SelectComponent(AppState state, SelectComponentAction action) =>
            ReduceComponentLookup(state, x => x
                .WithSelectedComponentNumber(action.Payload.ComponentNumber));

        private static AppState ReduceComponentLookup(AppState state, Func<ComponentLookup, ComponentLookup> reduce) =>
            state.WithShoppingSession(state.ShoppingSession
                .WithComponentLookup(reduce(state.ShoppingSession.ComponentLookup)));
    }
}
