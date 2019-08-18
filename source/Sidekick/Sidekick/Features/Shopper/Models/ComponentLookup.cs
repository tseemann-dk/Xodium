using System.Collections.Generic;
using System.Linq;

namespace Sidekick.Features.Shopper.Models
{
    public class ComponentLookup
    {
        public ComponentLookup(string searchText = null, IEnumerable<IComponentDescriptor> foundComponents = null)
        {
            SearchText = searchText;
            FoundComponents = foundComponents?.ToList() ?? new List<IComponentDescriptor>();
        }

        public string SearchText { get; }
        public IReadOnlyCollection<IComponentDescriptor> FoundComponents { get; }

        public ComponentLookup WithSearchText(string searchText) => 
            new ComponentLookup(searchText, FoundComponents); 

        public ComponentLookup WithFoundComponents(IEnumerable<IComponentDescriptor> foundComponents) => 
            new ComponentLookup(SearchText, foundComponents); 
    }
}
