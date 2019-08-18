using System;
using System.Collections.Generic;
using System.Linq;

namespace Xodium.Productivity.Content.Models
{
    public interface IBranch : INode
    {
        IReadOnlyList<INode> Nodes { get; }
    }

    public static class BranchExtensions
    {
        public static IBranch FindBranchOf(this IBranch root, INode node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            if (root.Nodes.Contains(node))
                return root;

            return root.Nodes
                .OfType<IBranch>()
                .Select(x => x.FindBranchOf(node))
                .FirstOrDefault(x => x != null);
        }

        public static INode FindNode(this IBranch root, Func<INode, bool> predicate)
            => FindNode<INode>(root, predicate);

        public static T FindNode<T>(this IBranch root, Func<T, bool> predicate)
            where T : INode
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            if (root is T node && predicate(node))
                return node;

            return root.Nodes
                .OfType<IBranch>()
                .Select(x => x.FindNode(predicate))
                .FirstOrDefault(x => x != null);
        }

        public static INode GetChildNode(this IBranch self, string nodeId)
        {
            return self.Nodes.FirstOrDefault(x => x.Id == nodeId) ?? throw new KeyNotFoundException($"Child node {nodeId} not found in branch {self.Id}");
        }

        public static int GetIndexOfNode(this IBranch self, INode node)
        {
            int index = 0;

            foreach (var item in self.Nodes)
            {
                if (item == node)
                    return index;

                index++;
            }

            return -1;
        }

        public static INode GetNextNode(this IBranch self, INode node)
        {
            return self.Nodes.SkipWhile(x => x != node).Skip(1).FirstOrDefault();
        }

        public static INode GetPreviousNode(this IBranch self, INode node)
        {
            var index = self.GetIndexOfNode(node);
            return index > 0 ? self.Nodes[index - 1] : null;
        }
    }
}
