using Xodium.Productivity.Content.Models;

namespace Sidekick.Features.Shopper.Models
{
    public class ShoppingSession
    {
        public ShoppingSession(ShoppingList shoppingList, string currentGroupId, string focusedNodeId)
        {
            ShoppingList = shoppingList ?? throw new System.ArgumentNullException(nameof(shoppingList));
            CurrentGroupId = currentGroupId;
            FocusedNodeId = focusedNodeId;
        }

        public ShoppingList ShoppingList { get; }
        public string CurrentGroupId { get; }
        public string FocusedNodeId { get; }

        public IShoppingGroup GetCurrentGroup() => ShoppingList.Content.FindNode<IShoppingGroup>(x => x.Id == CurrentGroupId);

        public ShoppingSession WithShoppingList(ShoppingList shoppingList) 
            => new ShoppingSession(shoppingList, CurrentGroupId, FocusedNodeId);

        public ShoppingSession WithCurrentGroupId(string currentGroupId, string focusedNodeId = null) 
            => new ShoppingSession(ShoppingList, currentGroupId, focusedNodeId);

        public ShoppingSession WithFocusedNodeId(string focusedNodeId) 
            => new ShoppingSession(ShoppingList, CurrentGroupId, focusedNodeId);
    }
}
