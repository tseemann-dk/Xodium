using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Xodium.DataStructures;

namespace Sidekick.Shopper.Models
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

        public string ThumbnailUrl => Component?.ThumbnailUrl;

        public ShoppingItem Clone() => new ShoppingItem(Id, Component, Quantity, Text, Price);

        private string DisplayNumber => ComponentNumber;
        private string DisplayText => Text ?? Component?.Text;

        INode INode.Clone() => Clone();

        string IShoppingNode.ReferenceNumber => DisplayNumber;
        string IShoppingNode.Text => DisplayText;
        double IShoppingNode.Price => Price ?? Component?.Price ?? 0;

        [ExcludeFromCodeCoverage]
        private string DebuggerDisplay => $"{DisplayNumber}, {DisplayText}";
    }
}
