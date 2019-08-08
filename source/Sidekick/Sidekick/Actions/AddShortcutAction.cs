using System;
using Xodium.Redux;

namespace Sidekick.Actions
{
    public class AddShortcutAction : ReduxAction<AddShortcutAction.Properties>
    {
        public AddShortcutAction(
            string parentFolderId, 
            DateTime date, 
            string text, 
            double quantity, 
            double value, 
            string insertAfterNodeId = null)
            : base(typeof(AddShortcutAction).FullName, new Properties(
                parentFolderId, 
                date, 
                text, 
                quantity, 
                value, 
                insertAfterNodeId
            ))
        {
        }

        public struct Properties
        {
            public Properties(
                string folderId,
                DateTime date, 
                string text, 
                double quantity, 
                double value, 
                string insertAfterNodeId)
            {
                ParentFolderId = folderId ?? throw new ArgumentNullException(nameof(folderId));
                Date = date;
                Text = text;
                Quantity = quantity;
                Value = value;
                InsertAfterNodeId = insertAfterNodeId;
            }

            public string ParentFolderId;
            public DateTime Date;
            public string Text;
            public double Quantity;
            public double Value;
            public string InsertAfterNodeId;
        }
    }
}
