using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Xodium.Productivity.Content.Models;

namespace Sidekick.Features.Shopper.Models
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class ShoppingItem : IShoppingItem
    {
        public ShoppingItem(IComponent component, double quantity, string text = null, double? value = null)
            : this(Guid.NewGuid().ToString(), component, quantity, text, value)
        {
        }

        public ShoppingItem(string id, IComponent component, double quantity, string text = null, double? price = null)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Component = component;
            ComponentNumber = component?.ComponentNumber;
            Quantity = quantity;
            Text = text;
            Price = price;
        }

        public string Id { get; }
        public string ComponentNumber { get; }
        public IComponent Component { get; }
        public string Text { get; }
        public double Quantity { get; }
        public double? Price { get; }

        private string DisplayNumber => ComponentNumber;
        private string DisplayText => Text ?? Component?.Text;

        string IShoppingNode.ReferenceNumber => DisplayNumber;
        string IShoppingNode.Text => DisplayText;
        double IShoppingNode.Price => Price ?? Component?.Price ?? 0;

        [ExcludeFromCodeCoverage]
        private string DebuggerDisplay => $"{DisplayNumber}, {DisplayText}";

        public INode Clone() => new ShoppingItem(Id, Component, Quantity, Text, Price);
    }
}
