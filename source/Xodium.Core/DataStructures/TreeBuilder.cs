using System;
using System.Collections.Generic;
using System.Linq;
using Xodium.DataStructures;

namespace Xodium.Core.Tests.Utilities
{
    public delegate T NodeCreator<T>(string id, IEnumerable<INode> nodes) where T : ITree;
    public delegate string IdentityProvider(string parentId, int index);
    public delegate IEnumerable<INode> NodesProvider();

    public class TreeBuilder<TNode>
        where TNode : class, ITree
    {
        private readonly NodeCreator<TNode> nodeCreator;
        private readonly IdentityProvider identityProvider;

        public TreeBuilder(NodeCreator<TNode> nodeCreator, IdentityProvider identityProvider = null)
        {
            this.nodeCreator = nodeCreator ?? throw new ArgumentNullException(nameof(nodeCreator));
            this.identityProvider = identityProvider ?? ((id, index) => $"{id}.{index}");
        }

        public TNode CreateNode(string id, IEnumerable<INode> children = null) => nodeCreator(id, children);
        public string ProvideId(string id, int index) => identityProvider(id, index);

        public TNode BuildTree(string id, int depth, int width, NodesProvider getLeaves = null)
        { 
            return CreateNode(id, depth > 0
                ? Enumerable.Range(1, width).Select(x => BuildTree(identityProvider(id, x), depth - 1, width, getLeaves))
                : getLeaves?.Invoke());
        }

        public TNode BuildTreeViaEvolution(string id, int depth, int width, Func<IEnumerable<INode>> getLeaves = null)
        {
            TNode node = CreateNode(id);

            if (depth > 0)
            {
                foreach (var x in Enumerable.Range(1, width))
                {
                    node = node.AddNode(
                        BuildTreeViaEvolution(ProvideId(id, x), depth - 1, width, getLeaves));
                }
            }
            else
            {
                var leaves = getLeaves?.Invoke();

                if (leaves != null)
                {
                    node = node.AddNodes(leaves);
                }
            }

            return node;
        }
    }
}
