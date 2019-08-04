using System;
using Xodium.Redux;

namespace Sidekick.Actions
{
    public class AddFolderAction : ReduxAction<AddFolderAction.Properties>
    {
        public AddFolderAction(
            string parentFolderId, 
            string number, 
            string text, 
            double quantity, 
            string insertAfterNodeId = null)
            : base(typeof(AddFolderAction).FullName, new Properties(
                parentFolderId, 
                number, 
                text, 
                quantity, 
                insertAfterNodeId
            ))
        {
        }

        public struct Properties
        {
            public Properties(
                string folderId, 
                string number, 
                string text, 
                double quantity, 
                string insertAfterNodeId)
            {
                ParentFolderId = folderId ?? throw new ArgumentNullException(nameof(folderId));
                Number = number;
                Text = text;
                Quantity = quantity;
                InsertAfterNodeId = insertAfterNodeId;
            }

            public string ParentFolderId;
            public string Number;
            public string Text;
            public double Quantity;
            public string InsertAfterNodeId;
        }
    }
}
