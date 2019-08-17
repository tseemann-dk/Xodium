using System;
using Xodium.Redux;

namespace Sidekick.Features.Shopper.Actions
{
    public class AddGroupAction : ReduxAction<AddGroupAction.Properties>
    {
        public AddGroupAction(
            string parentGroupId, 
            string groupNumber, 
            string text, 
            double quantity, 
            string insertAfterNodeId = null)
            : base(typeof(AddGroupAction).FullName, new Properties(
                parentGroupId, 
                groupNumber, 
                text, 
                quantity, 
                insertAfterNodeId
            ))
        {
        }

        public struct Properties
        {
            public Properties(
                string parentGroupId, 
                string groupNumber, 
                string text, 
                double quantity, 
                string insertAfterNodeId)
            {
                ParentGroupId = parentGroupId ?? throw new ArgumentNullException(nameof(parentGroupId));
                GroupNumber = groupNumber;
                Text = text;
                Quantity = quantity;
                InsertAfterNodeId = insertAfterNodeId;
            }

            public string ParentGroupId;
            public string GroupNumber;
            public string Text;
            public double Quantity;
            public string InsertAfterNodeId;
        }
    }
}
