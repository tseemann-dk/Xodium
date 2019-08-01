using System;
using Sidekick.Models;
using Xodium.Redux;

namespace Sidekick.Actions
{
    public class AddLineAction : ReduxAction<AddLineAction.Properties>
    {
        public AddLineAction(string folderId, IElement element, double quantity = 1, string insertAfterNodeId = null)
            : base(typeof(AddLineAction).FullName, new Properties(folderId, element, quantity, insertAfterNodeId))
        {
        }

        public struct Properties
        {
            public Properties(string folderId, IElement element, double quantity, string insertAfterNodeId)
            {
                FolderId = folderId ?? throw new ArgumentNullException(nameof(folderId));
                Element = element ?? throw new ArgumentNullException(nameof(element));
                Quantity = quantity;
                InsertAfterNodeId = insertAfterNodeId;
            }

            public string FolderId;
            public IElement Element;
            public double Quantity;
            public string InsertAfterNodeId;
        }
    }
}
