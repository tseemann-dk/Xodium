using System.Collections.Generic;
using System.Linq;

namespace Sidekick.Features.Shopper.Models
{
    public class ComponentLookup
    {
        public ComponentLookup()
            : this(false, null, null, null, false)
        {
        }

        public ComponentLookup(
            bool isVisible, 
            string searchText, 
            string searchError,
            IReadOnlyCollection<IComponentDescriptor> foundComponents, 
            bool isSearching)
        {
            IsVisible = isVisible;
            SearchText = searchText;
            SearchError = searchError;
            FoundComponents = foundComponents;
            IsSearching = isSearching;
        }

        public IReadOnlyCollection<IComponentDescriptor> FoundComponents { get; }
        public bool IsSearching { get; }
        public bool IsVisible { get; }
        public string SearchError { get; }
        public string SearchText { get; }

        public ComponentLookup WithFoundComponents(IReadOnlyCollection<IComponentDescriptor> foundComponents) => 
            new ComponentLookup(IsVisible, SearchText, SearchError, foundComponents, false); 

        public ComponentLookup WithIsVisible(bool isVisible) => 
            new ComponentLookup(isVisible, SearchText, SearchError, FoundComponents, IsSearching); 

        public ComponentLookup WithIsSearching(bool isSearching) => 
            new ComponentLookup(IsVisible, SearchText, SearchError, FoundComponents, isSearching); 

        public ComponentLookup WithSearchText(string searchText) => 
            new ComponentLookup(IsVisible, searchText, SearchError, FoundComponents, IsSearching); 

        public ComponentLookup WithSearchError(string searchError) => 
            new ComponentLookup(IsVisible, SearchText, searchError, null, false); 
    }
}
