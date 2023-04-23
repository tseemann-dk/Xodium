using System;
using System.Collections.Generic;
using System.Linq;
using Xodium.DataStructures;

namespace Xodium.Core.Tests.Models
{
    public class SampleNode : IContainerNode
    {
        public SampleNode(string id, IEnumerable<INode> nodes)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Nodes = nodes?.ToList() ?? new List<INode>();
        }

        public static TreeBuilder<SampleNode> CreateTreeBuilder() 
            => new TreeBuilder<SampleNode>((id, nodes) => new SampleNode(id, nodes));

        public string Id { get; }
        public IReadOnlyList<INode> Nodes { get; }

        public IContainerNode WithNodes(IReadOnlyList<INode> nodes) 
            => new SampleNode(Id, nodes);

        public INode Clone() => WithNodes(Nodes);
    }
}
