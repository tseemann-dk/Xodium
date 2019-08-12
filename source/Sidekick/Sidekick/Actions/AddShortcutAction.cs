using Sidekick.Models;
using System;
using Xodium.Redux;

namespace Sidekick.Actions
{
    public class AddShortcutAction : ReduxAction<AddShortcutAction.Properties>
    {
        public AddShortcutAction(
            string parentFolderId, 
            IElement element, 
            double quantity, 
            string text = null, 
            double? value = null, 
            string insertAfterNodeId = null)
            : base(typeof(AddShortcutAction).FullName, new Properties(
                parentFolderId, 
                element, 
                quantity, 
                text, 
                value, 
                insertAfterNodeId
            ))
        {
        }

        public struct Properties
        {
            public Properties(
                string folderId,
                IElement element, 
                double quantity, 
                string text, 
                double? value, 
                string insertAfterNodeId)
            {
                ParentFolderId = folderId ?? throw new ArgumentNullException(nameof(folderId));
                Element = element;
                Quantity = quantity;
                Text = text;
                Value = value;
                InsertAfterNodeId = insertAfterNodeId;
            }

            public string ParentFolderId;
            public IElement Element;
            public double Quantity;
            public string Text;
            public double? Value;
            public string InsertAfterNodeId;
        }
    }
}
