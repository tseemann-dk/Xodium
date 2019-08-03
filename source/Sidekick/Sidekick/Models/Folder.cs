using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Xodium.Productivity.Content.Models;

namespace Sidekick.Models
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class Folder : IFolder
    {
        public Folder(string id, string text, double quantity = 1, IEnumerable<INode> nodes = null)
        {
            Id = id ?? throw new System.ArgumentNullException(nameof(id));
            Text = text;
            Quantity = quantity;
            Nodes = nodes as IReadOnlyList<INode> ?? (nodes == null ? new List<INode>() : new List<INode>(nodes));
        }

        public string Id { get; }
        public string Text { get; }
        public double Quantity { get; }
        public double Value { get; }
        public IReadOnlyList<INode> Nodes { get; }

        [ExcludeFromCodeCoverage]
        private string DebuggerDisplay => $"{Text}";

        public IFolder Clone(IEnumerable<INode> nodes) 
            => new Folder(Id, Text, Quantity, nodes);

        IContainer IContainer.Clone(IEnumerable<INode> nodes) => Clone(nodes);
        INode INode.Clone() => Clone(Nodes);
    }
}
