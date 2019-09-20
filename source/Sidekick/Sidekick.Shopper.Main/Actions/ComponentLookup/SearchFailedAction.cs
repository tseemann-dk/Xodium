using System;
using Xodium.Redux;

namespace Sidekick.Shopper.Actions.ComponentLookup
{
    public class SearchFailedAction : ReduxAction<SearchFailedAction.Properties>
    {
        public SearchFailedAction(Exception exception)
            : base(typeof(SearchFailedAction).FullName, new Properties(exception))
        {
        }

        public class Properties
        {
            public Properties(Exception exception)
            {
                Exception = exception;
            }

            public Exception Exception { get; }
        }
    }
}
