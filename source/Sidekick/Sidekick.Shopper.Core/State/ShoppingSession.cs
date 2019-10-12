using Sidekick.Shopper.Models;
using Xodium.Productivity.Content.Models;

namespace Sidekick.Shopper.State
{
    public class ShoppingSession
    {
        public ShoppingSession(ShoppingList shoppingList, string currentFolderId, string focusedNodeId, ComponentLookup componentLookup = null)
        {
            ShoppingList = shoppingList ?? throw new System.ArgumentNullException(nameof(shoppingList));
            ComponentLookup = componentLookup ?? new ComponentLookup();
            CurrentFolderId = currentFolderId;
            FocusedNodeId = focusedNodeId;
        }

        public ShoppingList ShoppingList { get; }
        public ComponentLookup ComponentLookup { get; }
        public string CurrentFolderId { get; }
        public string FocusedNodeId { get; }

        public IShoppingFolder GetCurrentFolder() => ShoppingList.Content.FindNode<IShoppingFolder>(x => x.Id == CurrentFolderId);

        public ShoppingSession WithShoppingList(ShoppingList shoppingList) =>
            shoppingList == ShoppingList ? this :
            new ShoppingSession(shoppingList, CurrentFolderId, FocusedNodeId, ComponentLookup);

        public ShoppingSession WithComponentLookup(ComponentLookup componentLookup) =>
            componentLookup == ComponentLookup ? this :
            new ShoppingSession(ShoppingList, CurrentFolderId, FocusedNodeId, componentLookup);

        public ShoppingSession WithCurrentFolderId(string currentFolderId) =>
            currentFolderId == CurrentFolderId ? this :
            new ShoppingSession(ShoppingList, currentFolderId, FocusedNodeId, ComponentLookup);

        public ShoppingSession WithFocusedNodeId(string focusedNodeId) =>
            focusedNodeId == FocusedNodeId ? this :
            new ShoppingSession(ShoppingList, CurrentFolderId, focusedNodeId, ComponentLookup);
    }
}
