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
        public static IAction Commit() => new CommitAction();
        public static IAction ComponentPicked() => new ComponentPickedAction();
        public static IAction Hide() => new HideAction();
        public static IAction SearchStarting() => new SearchStartingAction();
        public static IAction SearchCompleted(IEnumerable<IComponentDescriptor> result) => new SearchCompletedAction(result);
        public static IAction SearchFailed(Exception exception) => new SearchFailedAction(exception);
        public static IAction Show() => new ShowAction();

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
    }
}
