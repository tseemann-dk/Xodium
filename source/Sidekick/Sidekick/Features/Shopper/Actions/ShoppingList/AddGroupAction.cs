using Sidekick.Features.Shopper.Models;
using System;
using Xodium.Redux;

namespace Sidekick.Features.Shopper.Actions.ShoppingList
{
    public class AddGroupAction : ReduxAction<AddGroupAction.Properties>
    {
        public AddGroupAction(
            string parentGroupId, 
            IShoppingGroup group, 
            string insertAfterNodeId = null)
            : base(typeof(AddGroupAction).FullName, new Properties(
                parentGroupId, 
                group, 
                insertAfterNodeId
            ))
        {
        }

        public struct Properties
        {
            public Properties(
                string parentGroupId, 
                IShoppingGroup group, 
                string insertAfterNodeId)
            {
                ParentGroupId = parentGroupId ?? throw new ArgumentNullException(nameof(parentGroupId));
                Group = group;
                InsertAfterNodeId = insertAfterNodeId;
            }

            public string ParentGroupId;
            public IShoppingGroup Group;
            public string InsertAfterNodeId;
        }
    }
}
