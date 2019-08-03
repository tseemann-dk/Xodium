using System;
using Xodium.Redux;

namespace Sidekick.Actions
{
    public class AddLineAction : ReduxAction<AddLineAction.Properties>
    {
        public AddLineAction(string folderId, DateTime date, string text, double quantity, double value, string insertAfterNodeId = null)
            : base(typeof(AddLineAction).FullName, new Properties(folderId, date, text, quantity, value, insertAfterNodeId))
        {
        }

        public struct Properties
        {
            public Properties(string folderId, DateTime date, string text, double quantity, double value, string insertAfterNodeId)
            {
                FolderId = folderId ?? throw new ArgumentNullException(nameof(folderId));
                Date = date;
                Text = text;
                Quantity = quantity;
                Value = value;
                InsertAfterNodeId = insertAfterNodeId;
            }

            public string FolderId;
            public DateTime Date;
            public string Text;
            public double Quantity;
            public double Value;
            public string InsertAfterNodeId;
        }
    }
}
