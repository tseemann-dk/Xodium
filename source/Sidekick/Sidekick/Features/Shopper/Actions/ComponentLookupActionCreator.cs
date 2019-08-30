using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sidekick.Features.Shopper.Actions.ComponentLookup;
using Sidekick.Features.Shopper.Models;
using Sidekick.State;
using Xodium.Flow;

namespace Sidekick.Features.Shopper.Actions
{
    public static class ComponentLookupActionCreator
    {
        public static IAction ChangeSearchText(string newSearchText) => new ChangeSearchTextAction(newSearchText);
        public static IAction Commit() => new PickComponentAction();
        public static IAction ComponentPicked(IComponentDescriptor component) => new ComponentPickedAction(component);
        public static IAction HideLookup() => new HideLookupAction();
        public static IAction Search(string searchText) => new SearchAction(searchText);
        public static IAction SearchStarting() => new SearchStartingAction();
        public static IAction SearchCompleted(IEnumerable<IComponentDescriptor> result) => new SearchCompletedAction(result);
        public static IAction SearchFailed(Exception exception) => new SearchFailedAction(exception);
        public static IAction SelectComponent(string componentNumber) => new SelectComponentAction(componentNumber);
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
