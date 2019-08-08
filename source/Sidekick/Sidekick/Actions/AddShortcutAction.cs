using Sidekick.Models;
using System;
using Xodium.Redux;

namespace Sidekick.Actions
{
    public class AddShortcutAction : ReduxAction<AddShortcutAction.Properties>
    {
        public AddShortcutAction(
            string parentFolderId, 
            IElement target, 
            double quantity, 
            string text = null, 
            double? value = null, 
            string insertAfterNodeId = null)
            : base(typeof(AddShortcutAction).FullName, new Properties(
                parentFolderId, 
                target, 
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
                IElement target, 
                double quantity, 
                string text, 
                double? value, 
                string insertAfterNodeId)
            {
                ParentFolderId = folderId ?? throw new ArgumentNullException(nameof(folderId));
                Target = target;
                Quantity = quantity;
                Text = text;
                Value = value;
                InsertAfterNodeId = insertAfterNodeId;
            }

            public string ParentFolderId;
            public IElement Target;
            public double Quantity;
            public string Text;
            public double? Value;
            public string InsertAfterNodeId;
        }
    }
}
