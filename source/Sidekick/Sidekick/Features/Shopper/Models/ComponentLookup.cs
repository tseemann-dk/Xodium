using System.Collections.Generic;
using System.Linq;

namespace Sidekick.Features.Shopper.Models
{
    public class ComponentLookup
    {
        public ComponentLookup(bool isVisible = false, string searchText = null, IEnumerable<IComponentDescriptor> foundComponents = null)
        {
            IsVisible = isVisible;
            SearchText = searchText;
            FoundComponents = foundComponents?.ToList() ?? new List<IComponentDescriptor>();
        }

        public IReadOnlyCollection<IComponentDescriptor> FoundComponents { get; }
        public bool IsVisible { get; }
        public string SearchText { get; }

        public ComponentLookup WithFoundComponents(IEnumerable<IComponentDescriptor> foundComponents) => 
            new ComponentLookup(IsVisible, SearchText, foundComponents); 

        public ComponentLookup WithIsVisible(bool isVisible) => 
            new ComponentLookup(isVisible, SearchText, FoundComponents); 

        public ComponentLookup WithSearchText(string searchText) => 
            new ComponentLookup(IsVisible, searchText, FoundComponents); 

    }
}
