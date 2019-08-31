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

        public ShoppingSession WithShoppingList(ShoppingList shoppingList) =>
            shoppingList == ShoppingList ? this :
            new ShoppingSession(shoppingList, CurrentGroupId, FocusedNodeId, ComponentLookup);

        public ShoppingSession WithComponentLookup(ComponentLookup componentLookup) =>
            componentLookup == ComponentLookup ? this :
            new ShoppingSession(ShoppingList, CurrentGroupId, FocusedNodeId, componentLookup);

        public ShoppingSession WithCurrentGroupId(string currentGroupId) =>
            currentGroupId == CurrentGroupId ? this :
            new ShoppingSession(ShoppingList, currentGroupId, FocusedNodeId, ComponentLookup);

        public ShoppingSession WithFocusedNodeId(string focusedNodeId) =>
            focusedNodeId == FocusedNodeId ? this :
            new ShoppingSession(ShoppingList, CurrentGroupId, focusedNodeId, ComponentLookup);
    }
}
