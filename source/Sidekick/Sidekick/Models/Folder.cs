using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Xodium.Productivity.Content.Models;

namespace Sidekick.Models
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class Folder : IFolder
    {
        public Folder(string number, string text, double quantity)
            : this(Guid.NewGuid().ToString(), number, text, quantity)
        {
        }

        public Folder(string id, string number, string text, double quantity = 1, IEnumerable<INode> nodes = null)
        {
            Id = id ?? throw new System.ArgumentNullException(nameof(id));
            Number = number ?? throw new ArgumentNullException(nameof(number));
            Text = text;
            Quantity = quantity;
            Nodes = nodes as IReadOnlyList<INode> ?? (nodes == null ? new List<INode>() : new List<INode>(nodes));
        }

        public string Id { get; }
        public string Number { get; }
        public string Text { get; }
        public double Quantity { get; }
        public double Value { get; }
        public IReadOnlyList<INode> Nodes { get; }

        string IArchiveNode.ReferenceNumber => Number;

        [ExcludeFromCodeCoverage]
        private string DebuggerDisplay => $"{Text}";

        public IFolder Clone(IEnumerable<INode> nodes) 
            => new Folder(Id, Number, Text, Quantity, nodes);

        IContainer IContainer.Clone(IEnumerable<INode> nodes) => Clone(nodes);
        INode INode.Clone() => Clone(Nodes);
    }
}
