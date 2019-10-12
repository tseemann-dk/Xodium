using System;
using Sidekick.Shopper.Models;
using Xodium.Redux;

namespace Sidekick.Shopper.Actions.ShoppingList
{
    public class AddItemAction : ReduxAction<AddItemAction.Properties>
    {
        public AddItemAction(
            string folderId, 
            IShoppingItem item, 
            string insertAfterNodeId = null)
            : base(typeof(AddItemAction).FullName, new Properties(
                folderId, 
                item, 
                insertAfterNodeId
            ))
        {
        }

        public struct Properties
        {
            public Properties(
                string folderId,
                IShoppingItem item, 
                string insertAfterNodeId)
            {
                FolderId = folderId ?? throw new ArgumentNullException(nameof(folderId));
                Item = item;
                InsertAfterNodeId = insertAfterNodeId;
            }

            public string FolderId;
            public IShoppingItem Item;
            public string InsertAfterNodeId;
        }
    }
}
