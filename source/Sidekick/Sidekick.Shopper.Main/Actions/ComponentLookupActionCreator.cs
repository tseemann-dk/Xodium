using System;
using System.Collections.Generic;
using Sidekick.Shopper.Actions.ComponentLookup;
using Sidekick.Shopper.Models;
using Xodium.Flow;

namespace Sidekick.Shopper.Actions
{
    public static class ComponentLookupActionCreator
    {
        public static IAction ComponentPicked(IComponentDescriptor component) => new ComponentPickedAction(component);
        public static IAction HideLookup() => new HideLookupAction();
        public static IAction PickComponent() => new PickComponentAction();
        public static IAction Search(IShop shop, string searchText = null) => new SearchAction(searchText, shop);
        public static IAction SearchStarting() => new SearchStartingAction();
        public static IAction SearchCompleted(IEnumerable<IComponentDescriptor> result) => new SearchCompletedAction(result);
        public static IAction SearchFailed(Exception exception) => new SearchFailedAction(exception);
        public static IAction SelectComponent(string componentNumber) => new SelectComponentAction(componentNumber);
        public static IAction SetSearchText(string newSearchText) => new SetSearchTextAction(newSearchText);
        public static IAction ShowLookup() => new ShowLookupAction();

        /*
        public static ActionsCreator<AppState> Search()
        {
            return async (dispatch, getState) =>
            {
                dispatch(SearchStarting());
                try
                {
                    await Task.Delay(2000);

                    dispatch(SearchCompleted(new[] 
                    {
                        new ComponentDescriptor(new ComponentReference(ShopIdentity.Internal, "C001"), "First Component", 10),
                        new ComponentDescriptor(new ComponentReference(ShopIdentity.Internal, "C002"), "Second Component", 20),
                        new ComponentDescriptor(new ComponentReference(ShopIdentity.Internal, "C003"), "Third Component", 30)
                    }));
                }
                catch (Exception exception)
                {
                    dispatch(SearchFailed(exception));
                }
            };
        }
        */
    }
}
