using System;
using Sidekick.Features.Shopper.Models;
using Xodium.Redux;

namespace Sidekick.Features.Shopper.Actions.ShoppingList
{
    public class AddItemAction : ReduxAction<AddItemAction.Properties>
    {
        public AddItemAction(
            string parentGroupId, 
            IShoppingItem item, 
            string insertAfterNodeId = null)
            : base(typeof(AddItemAction).FullName, new Properties(
                parentGroupId, 
                item, 
                insertAfterNodeId
            ))
        {
        }

        public struct Properties
        {
            public Properties(
                string parentGroupId,
                IShoppingItem item, 
                string insertAfterNodeId)
            {
                ParentGroupId = parentGroupId ?? throw new ArgumentNullException(nameof(parentGroupId));
                Item = item;
                InsertAfterNodeId = insertAfterNodeId;
            }

            public string ParentGroupId;
            public IShoppingItem Item;
            public string InsertAfterNodeId;
        }
    }
}
