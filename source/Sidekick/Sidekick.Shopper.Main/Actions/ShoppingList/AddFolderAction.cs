using Sidekick.Shopper.Models;
using System;
using Xodium.Redux;

namespace Sidekick.Shopper.Actions.ShoppingList
{
    public class AddFolderAction : ReduxAction<AddFolderAction.Properties>
    {
        public AddFolderAction(
            string parentFolderId, 
            IShoppingFolder folder, 
            string insertAfterNodeId = null)
            : base(typeof(AddFolderAction).FullName, new Properties(
                parentFolderId, 
                folder, 
                insertAfterNodeId
            ))
        {
        }

        public struct Properties
        {
            public Properties(
                string parentFolderId, 
                IShoppingFolder folder, 
                string insertAfterNodeId)
            {
                ParentFolderId = parentFolderId ?? throw new ArgumentNullException(nameof(parentFolderId));
                Folder = folder;
                InsertAfterNodeId = insertAfterNodeId;
            }

            public string ParentFolderId;
            public IShoppingFolder Folder;
            public string InsertAfterNodeId;
        }
    }
}
