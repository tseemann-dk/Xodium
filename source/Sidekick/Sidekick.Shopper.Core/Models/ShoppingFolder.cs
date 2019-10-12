using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Xodium.Productivity.Content.Models;

namespace Sidekick.Shopper.Models
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class ShoppingFolder : IShoppingFolder
    {
        public ShoppingFolder(string number, string text, double quantity)
            : this(Guid.NewGuid().ToString(), number, text, quantity)
        {
        }

        public ShoppingFolder(string id, string number, string title, double quantity = 1, IReadOnlyList<INode> nodes = null)
        {
            Id = id ?? throw new System.ArgumentNullException(nameof(id));
            FolderNumber = number ?? throw new ArgumentNullException(nameof(number));
            Title = title;
            Quantity = quantity;
            Nodes = nodes ?? new List<INode>();
            Price = Nodes.OfType<IShoppingNode>().Sum(x => x.Price);
        }

        public string Id { get; }
        public string FolderNumber { get; }
        public string Title { get; }
        public double Quantity { get; }
        public double Price { get; }
        public IReadOnlyList<INode> Nodes { get; }

        string IShoppingNode.ReferenceNumber => FolderNumber;
        string IShoppingNode.Text => Title;

        [ExcludeFromCodeCoverage]
        private string DebuggerDisplay => $"{Title}";

        public IShoppingFolder WithTitle(string title)
            => new ShoppingFolder(Id, FolderNumber, title, Quantity, Nodes);

        public IShoppingFolder WithNodes(IReadOnlyList<INode> nodes) 
            => new ShoppingFolder(Id, FolderNumber, Title, Quantity, nodes);

        ITree ITree.WithNodes(IReadOnlyList<INode> nodes) => WithNodes(nodes);
        INode INode.Clone() => WithNodes(Nodes);
    }
}
