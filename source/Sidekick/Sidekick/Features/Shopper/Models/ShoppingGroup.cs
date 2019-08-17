using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Xodium.Productivity.Content.Models;

namespace Sidekick.Features.Shopper.Models
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class ShoppingGroup : IShoppingGroup
    {
        public ShoppingGroup(string number, string text, double quantity)
            : this(Guid.NewGuid().ToString(), number, text, quantity)
        {
        }

        public ShoppingGroup(string id, string number, string title, double quantity = 1, IEnumerable<INode> nodes = null)
        {
            Id = id ?? throw new System.ArgumentNullException(nameof(id));
            GroupNumber = number ?? throw new ArgumentNullException(nameof(number));
            Title = title;
            Quantity = quantity;
            Nodes = nodes as IReadOnlyList<INode> ?? (nodes == null ? new List<INode>() : new List<INode>(nodes));
            Price = Nodes.OfType<IShoppingNode>().Sum(x => x.Price);
        }

        public string Id { get; }
        public string GroupNumber { get; }
        public string Title { get; }
        public double Quantity { get; }
        public double Price { get; }
        public IReadOnlyList<INode> Nodes { get; }

        string IShoppingNode.ReferenceNumber => GroupNumber;
        string IShoppingNode.Text => Title;

        [ExcludeFromCodeCoverage]
        private string DebuggerDisplay => $"{Title}";

        public IShoppingGroup WithNodes(IEnumerable<INode> nodes) 
            => new ShoppingGroup(Id, GroupNumber, Title, Quantity, nodes);

        IContainer IContainer.WithNodes(IEnumerable<INode> nodes) => WithNodes(nodes);
        INode INode.Clone() => WithNodes(Nodes);
    }
}
