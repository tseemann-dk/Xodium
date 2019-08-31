using Sidekick.Shopper.Models;
using Xodium.Productivity.Content.Models;

namespace Sidekick.Shopper.State
{
    public class ShoppingSession
    {
        public ShoppingSession(ShoppingList shoppingList, string currentGroupId, string focusedNodeId, ComponentLookup componentLookup = null)
        {
            ShoppingList = shoppingList ?? throw new System.ArgumentNullException(nameof(shoppingList));
            ComponentLookup = componentLookup ?? new ComponentLookup();
            CurrentGroupId = currentGroupId;
            FocusedNodeId = focusedNodeId;
        }

        public ShoppingList ShoppingList { get; }
        public ComponentLookup ComponentLookup { get; }
        public string CurrentGroupId { get; }
        public string FocusedNodeId { get; }

        public IShoppingGroup GetCurrentGroup() => ShoppingList.Content.FindNode<IShoppingGroup>(x => x.Id == CurrentGroupId);

        public ShoppingSession WithShoppingList(ShoppingList shoppingList) 
            => new ShoppingSession(shoppingList, CurrentGroupId, FocusedNodeId, ComponentLookup);

        public ShoppingSession WithComponentLookup(ComponentLookup componentLookup)
            => new ShoppingSession(ShoppingList, CurrentGroupId, FocusedNodeId, componentLookup);

        public ShoppingSession WithCurrentGroupId(string currentGroupId, string focusedNodeId = null) 
            => new ShoppingSession(ShoppingList, currentGroupId, focusedNodeId, ComponentLookup);

        public ShoppingSession WithFocusedNodeId(string focusedNodeId) 
            => new ShoppingSession(ShoppingList, CurrentGroupId, focusedNodeId, ComponentLookup);
    }
}
