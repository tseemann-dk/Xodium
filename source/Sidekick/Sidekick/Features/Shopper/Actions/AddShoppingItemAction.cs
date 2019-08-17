using System;
using Sidekick.Features.Shopper.Models;
using Xodium.Redux;

namespace Sidekick.Features.Shopper.Actions
{
    public class AddShoppingItemAction : ReduxAction<AddShoppingItemAction.Properties>
    {
        public AddShoppingItemAction(
            string parentGroupId, 
            IComponent component, 
            double quantity, 
            string text = null, 
            double? value = null, 
            string insertAfterNodeId = null)
            : base(typeof(AddShoppingItemAction).FullName, new Properties(
                parentGroupId, 
                component, 
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
                string parentGroupId,
                IComponent component, 
                double quantity, 
                string text, 
                double? value, 
                string insertAfterNodeId)
            {
                ParentGroupId = parentGroupId ?? throw new ArgumentNullException(nameof(parentGroupId));
                Component = component;
                Quantity = quantity;
                Text = text;
                Value = value;
                InsertAfterNodeId = insertAfterNodeId;
            }

            public string ParentGroupId;
            public IComponent Component;
            public double Quantity;
            public string Text;
            public double? Value;
            public string InsertAfterNodeId;
        }
    }
}
