using System.Collections.Generic;
using System.Linq;

namespace Sidekick.Features.Shopper.Models
{
    public class ShopVisit
    {
        public ShopVisit(string searchText = null, IEnumerable<IComponentDescriptor> foundComponents = null)
        {
            SearchText = searchText;
            FoundComponents = foundComponents?.ToList() ?? new List<IComponentDescriptor>();
        }

        public string SearchText { get; }
        public IReadOnlyCollection<IComponentDescriptor> FoundComponents { get; }

        public ShopVisit WithSearchText(string searchText) => 
            new ShopVisit(searchText, FoundComponents); 

        public ShopVisit WithFoundComponents(IEnumerable<IComponentDescriptor> foundComponents) => 
            new ShopVisit(SearchText, foundComponents); 
    }
}
