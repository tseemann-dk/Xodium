using System;
using System.Collections.Generic;
using Sidekick.Features.Shopper.Actions.ComponentLookup;
using Sidekick.Features.Shopper.Models;
using Xodium.Flow;

namespace Sidekick.Features.Shopper.Reducers
{
    public static class ComponentLookupReducer
    {
        private static readonly Dictionary<Type, Reducer<ComponentLookup>> handlers = new Dictionary<Type, Reducer<ComponentLookup>>
        {
            [typeof(ChangeSearchTextAction)] = (s, a) => ChangeSearchText(s, (ChangeSearchTextAction)a),
            [typeof(SearchCompletedAction)] = (s, a) => SearchCompleted(s, (SearchCompletedAction)a),
            [typeof(SearchFailedAction)] = (s, a) => SearchFailed(s, (SearchFailedAction)a),
            [typeof(SearchStartingAction)] = (s, _) => SearchStarting(s),
            [typeof(SelectComponentAction)] = (s, a) => SelectComponent(s, (SelectComponentAction)a),
            [typeof(ShowAction)] = (s, a) => ChangeIsVisible(s, true),
            [typeof(HideAction)] = (s, a) => ChangeIsVisible(s, false)
        };

        public static ComponentLookup Execute(ComponentLookup state, object action) =>
            handlers.TryGetValue(action.GetType(), out var handler) ? handler(state, action) : state;

        private static ComponentLookup ChangeSearchText(ComponentLookup state, ChangeSearchTextAction action) => 
            state.WithSearchText(action.Payload.NewSearchText);

        private static ComponentLookup ChangeIsVisible(ComponentLookup state, bool isVisible) =>
            state.WithIsVisible(isVisible);

        private static ComponentLookup SearchCompleted(ComponentLookup state, SearchCompletedAction action) =>
            state
                .WithFoundComponents(action.Payload.Result)
                .WithSearchError(null)
                .WithIsSearching(false);

        private static ComponentLookup SearchFailed(ComponentLookup state, SearchFailedAction action) =>
            state
                .WithFoundComponents(null)
                .WithSearchError(action.Payload.Exception.Message)
                .WithIsSearching(false);

        private static ComponentLookup SearchStarting(ComponentLookup state) =>
            state
                .WithSearchError(null)
                .WithFoundComponents(null)
                .WithIsSearching(true);

        private static ComponentLookup SelectComponent(ComponentLookup state, SelectComponentAction action) =>
            state.WithSelectedComponentNumber(action.Payload.ComponentNumber);
    }
}
