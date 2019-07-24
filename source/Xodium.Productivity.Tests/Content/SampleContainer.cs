using System;
using System.Collections.Generic;
using System.Linq;
using Xodium.Productivity.Content.Models;

namespace Xodium.Productivity.Tests.Content
{
    public class SampleContainer : IContainer
    {
        public SampleContainer(string id, IEnumerable<INode> nodes)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Nodes = nodes?.ToList() ?? new List<INode>();
        }

        public string Id { get; }
        public IReadOnlyList<INode> Nodes { get; }

        public IContainer Clone(IEnumerable<INode> nodes) 
            => new SampleContainer(Id, nodes);

        public INode Clone() => Clone(Nodes);
    }
}
