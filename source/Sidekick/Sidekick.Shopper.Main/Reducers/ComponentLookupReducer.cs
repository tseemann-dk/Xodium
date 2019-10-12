using System;
using System.Collections.Generic;
using Sidekick.Shopper.Actions.ComponentLookup;
using Sidekick.Shopper.State;
using Xodium.Flow;

namespace Sidekick.Shopper.Reducers
{
    public static class ComponentLookupReducer
    {
        private static readonly Dictionary<Type, Reducer<ComponentLookup>> handlers = new Dictionary<Type, Reducer<ComponentLookup>>
        {
            [typeof(SetSearchTextAction)] = (s, a) => ChangeSearchText(s, (SetSearchTextAction)a),
            [typeof(SearchCompletedAction)] = (s, a) => SearchCompleted(s, (SearchCompletedAction)a),
            [typeof(SearchFailedAction)] = (s, a) => SearchFailed(s, (SearchFailedAction)a),
            [typeof(SearchStartingAction)] = (s, _) => SearchStarting(s),
            [typeof(SelectComponentAction)] = (s, a) => SelectComponent(s, (SelectComponentAction)a),
            [typeof(ShowLookupAction)] = (s, a) => ChangeIsVisible(s, true),
            [typeof(HideLookupAction)] = (s, a) => ChangeIsVisible(s, false)
        };

        public static ComponentLookup Reduce(ComponentLookup state, object action)
        {
            if (handlers.TryGetValue(action.GetType(), out var handler))
            {
                return handler(state, action);
            }

            return state;
        }

        private static ComponentLookup ChangeSearchText(ComponentLookup state, SetSearchTextAction action) => 
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
                .WithFoundComponents(null)
                .WithSearchError(null)
                .WithIsSearching(true);

        private static ComponentLookup SelectComponent(ComponentLookup state, SelectComponentAction action) =>
            state.WithSelectedComponentNumber(action.Payload.ComponentNumber);
    }
}
