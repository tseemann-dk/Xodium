using System.Collections.Generic;
using System.Linq;

namespace Sidekick.Features.Shopper.Models
{
    public class ComponentLookup
    {
        public ComponentLookup()
            : this(false, false, null, null, null, null)
        {
        }

        public ComponentLookup(
            bool isVisible,
            bool isSearching,
            string searchText,
            string searchError,
            IReadOnlyCollection<IComponentDescriptor> foundComponents,
            string selectedComponentNumber)
        {
            IsVisible = isVisible;
            IsSearching = isSearching;
            SearchText = searchText;
            SearchError = searchError;
            FoundComponents = foundComponents;
            SelectedComponentNumber = selectedComponentNumber;
        }

        public IReadOnlyCollection<IComponentDescriptor> FoundComponents { get; }
        public bool IsSearching { get; }
        public bool IsVisible { get; }
        public string SearchError { get; }
        public string SearchText { get; }
        public string SelectedComponentNumber { get; }

        public IComponentDescriptor GetSelectedComponent() => SelectedComponentNumber == null ? null : FoundComponents?.FirstOrDefault(x => x.Reference.ComponentNumber == SelectedComponentNumber);

        public ComponentLookup WithFoundComponents(IReadOnlyCollection<IComponentDescriptor> foundComponents) => 
            new ComponentLookup(IsVisible, false, SearchText, SearchError, foundComponents, SelectedComponentNumber); 

        public ComponentLookup WithIsVisible(bool isVisible) => 
            new ComponentLookup(isVisible, IsSearching, SearchText, SearchError, FoundComponents, SelectedComponentNumber); 

        public ComponentLookup WithIsSearching(bool isSearching) => 
            new ComponentLookup(IsVisible, isSearching, SearchText, SearchError, FoundComponents, SelectedComponentNumber); 

        public ComponentLookup WithSearchText(string searchText) => 
            new ComponentLookup(IsVisible, IsSearching, searchText, SearchError, FoundComponents, SelectedComponentNumber); 

        public ComponentLookup WithSearchError(string searchError) => 
            new ComponentLookup(IsVisible, false, SearchText, searchError, null, SelectedComponentNumber); 

        public ComponentLookup WithSelectedComponentNumber(string selectedComponentId) => 
            new ComponentLookup(IsVisible, IsSearching, SearchText, SearchError, FoundComponents, selectedComponentId); 
    }
}
