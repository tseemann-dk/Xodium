using System.Collections.Generic;
using System.Linq;
using Sidekick.Shopper.Models;
using Xodium.Redux;

namespace Sidekick.Shopper.Actions.ComponentLookup
{
    public class SearchCompletedAction : ReduxAction<SearchCompletedAction.Properties>
    {
        public SearchCompletedAction(IEnumerable<IComponentDescriptor> result)
            : base(typeof(SearchCompletedAction).FullName, new Properties(result))
        {
        }

        public class Properties
        {
            public Properties(IEnumerable<IComponentDescriptor> result)
            {
                Result = result?.ToList() ?? new List<IComponentDescriptor>();
            }

            public IReadOnlyList<IComponentDescriptor> Result { get; }
        }
    }
}
