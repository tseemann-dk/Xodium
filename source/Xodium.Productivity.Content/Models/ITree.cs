using System;
using System.Collections.Generic;
using System.Linq;

namespace Xodium.Productivity.Content.Models
{
    public interface ITree : INode
    {
        IReadOnlyList<INode> Nodes { get; }

        ITree WithNodes(IReadOnlyList<INode> nodes);
    }

    public static class TreeExtensions
    {
        #region Find & Inspect Methods

        public static ITree FindParentOf(this ITree root, INode node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            if (root.Nodes.Contains(node))
                return root;

            return root.Nodes
                .OfType<ITree>()
                .Select(x => x.FindParentOf(node))
                .FirstOrDefault(x => x != null);
        }

        public static INode FindNode(this ITree root, Func<INode, bool> predicate)
            => FindNode<INode>(root, predicate);

        public static T FindNode<T>(this ITree root, Func<T, bool> predicate)
            where T : INode
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            if (root is T node && predicate(node))
                return node;

            return root.Nodes
                .OfType<ITree>()
                .Select(x => x.FindNode(predicate))
                .FirstOrDefault(x => x != null);
        }

        public static INode GetChildNode(this ITree self, string nodeId)
        {
            return self.Nodes.FirstOrDefault(x => x.Id == nodeId) ?? throw new KeyNotFoundException($"Child node {nodeId} not found at branch node {self.Id}");
        }

        public static int GetIndexOfNode(this ITree self, INode node)
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

        public static INode GetNextSibling(this ITree self, INode node)
        {
            return self.Nodes.SkipWhile(x => x != node).Skip(1).FirstOrDefault();
        }

        public static INode GetPreviousSibling(this ITree self, INode node)
        {
            var index = self.GetIndexOfNode(node);
            return index > 0 ? self.Nodes[index - 1] : null;
        }

        public static IEnumerable<ITree> GetSubTrees(this ITree self)
            => self.Nodes.OfType<ITree>();

        #endregion

        #region Add Methods

        public static T AddNode<T>(this T self, INode node)
            where T : class, ITree
            => self.AddNodes(new[] { node });

        public static T AddNodeAt<T>(this T self, ITree parent, INode node)
            where T : class, ITree
            => self.AddNodesAt(parent, new[] { node });

        public static T AddNodes<T>(this T self, IEnumerable<INode> nodes)
            where T : class, ITree
            => self.WithNodes(self.Nodes.Concat(nodes).ToList()) as T;

        public static T AddNodesAt<T>(this T self, ITree parent, IEnumerable<INode> nodes)
            where T : class, ITree
        {
            if (self.TryAddNodes(parent, nodes, out var result))
            {
                return result as T;
            }

            throw new ArgumentException("Not found", nameof(parent));
        }

        #endregion

        #region Insert Methods

        public static T InsertNode<T>(this T self, int index, INode node)
            where T : class, ITree
            => self.InsertNodes(index, new[] { node });

        public static T InsertNodeAt<T>(this T self, ITree parent, int index, INode node)
            where T : class, ITree
            => self.InsertNodesAt(parent, index, new[] { node });

        public static T InsertNodes<T>(this T self, int index, IEnumerable<INode> nodes)
            where T : class, ITree
        {
            var newNodes = self.Nodes.ToList();
            newNodes.InsertRange(index, nodes);
            return self.WithNodes(newNodes) as T;
        }

        public static T InsertNodesAt<T>(this T self, ITree parent, int index, IEnumerable<INode> nodes)
            where T : class, ITree
        {
            if (self.TryInsertNodes(parent, index, nodes, out var result))
            {
                return result as T;
            }

            throw new ArgumentException("Not found", nameof(parent));
        }

        #endregion

        #region Remove Methods

        public static T RemoveNode<T>(this T self, INode oldNode)
            where T : class, ITree
        {
            if (oldNode.IsChildOf(self))
            {
                return self.RemoveChildNode(oldNode);
            }

            if (oldNode.GetParent(self) is ITree parent)
            {
                return self.RemoveNodeAt(parent, oldNode);
            }

            throw new ArgumentException("Not found", nameof(oldNode));
        }

        private static T RemoveChildNode<T>(this T self, INode oldNode)
            where T : class, ITree
        {
            var nodes = self.Nodes.ToList();
            nodes.Remove(oldNode);
            return self.WithNodes(nodes) as T;
        }

        public static T RemoveNodeAt<T>(this T self, ITree parent, INode oldNode)
            where T : class, ITree
        {
            if (self.TryRemoveNode(parent, oldNode, out var result))
            {
                return result as T;
            }

            throw new ArgumentException("Not found", nameof(oldNode));
        }

        public static T RemoveChildNodes<T>(this T self, IEnumerable<INode> oldNodes)
            where T : class, ITree
        {
            var nodes = self.Nodes.ToList();
            nodes.RemoveAll(x => oldNodes.Contains(x));
            return self.WithNodes(nodes) as T;
        }

        public static T ReplaceNode<T>(this T self, INode oldNode, INode newNode)
            where T : class, ITree
        {
            if (self.Nodes.Contains(oldNode))
            {
                return self.ReplaceChildNode(oldNode, newNode);
            }

            if (oldNode.GetParent(self) is ITree parent)
            {
                return self.ReplaceNodeAt(parent, oldNode, newNode);
            }

            throw new ArgumentException("Not found", nameof(oldNode));
        }

        #endregion

        #region Replace Methods

        private static T ReplaceChildNode<T>(this T self, INode oldNode, INode newNode)
            where T : class, ITree
        {
            var nodes = self.Nodes.ToList();
            var index = nodes.IndexOf(oldNode);

            if (index < 0)
                throw new ArgumentException("Not found", nameof(oldNode));

            nodes.Insert(index, newNode);
            nodes.Remove(oldNode);

            return self.WithNodes(nodes) as T;
        }

        public static T ReplaceNodeAt<T>(this T self, ITree parent, INode oldNode, INode newNode)
            where T : class, ITree
        {
            if (self.TryReplaceNode(parent, oldNode, newNode, out var result))
            {
                return result as T;
            }

            throw new ArgumentException("Not found", nameof(oldNode));
        }

        public static T SwapChildNodes<T>(this T self, INode node1, INode node2)
            where T : class, ITree
        {
            var nodes = self.Nodes.ToList();
            var index1 = nodes.IndexOf(node1);
            var index2 = nodes.IndexOf(node2);

            if (index1 < 0)
                throw new ArgumentException("Not found", nameof(node1));

            if (index2 < 0)
                throw new ArgumentException("Not found", nameof(node2));

            nodes[index1] = node2;
            nodes[index2] = node1;

            return self.WithNodes(nodes) as T;
        }

        #endregion

        #region Internal Methods

        private static bool TryAddNodes<T>(this T self, ITree parent, IEnumerable<INode> nodes, out T result)
            where T : class, ITree
        {
            if (self == parent)
            {
                result = self.AddNodes(nodes);
                return true;
            }

            foreach (var branchNode in self.GetSubTrees())
            {
                if (branchNode.TryAddNodes(parent, nodes, out var branch))
                {
                    result = self.ReplaceChildNode(branchNode, branch);
                    return true;
                }
            }

            result = null;
            return false;
        }

        private static bool TryInsertNodes<T>(this T self, ITree parent, int index, IEnumerable<INode> nodes, out T result)
            where T : class, ITree
        {
            if (self == parent)
            {
                result = self.InsertNodes(index, nodes);
                return true;
            }

            foreach (var branchNode in self.GetSubTrees())
            {
                if (branchNode.TryInsertNodes(parent, index, nodes, out var branch))
                {
                    result = self.ReplaceChildNode(branchNode, branch);
                    return true;
                }
            }

            result = null;
            return false;
        }

        private static bool TryRemoveNode<T>(this T self, ITree parent, INode oldNode, out T result)
            where T : class, ITree
        {
            if (self == parent)
            {
                result = self.RemoveChildNode(oldNode);
                return true;
            }

            foreach (var branchNode in self.GetSubTrees())
            {
                if (branchNode.TryRemoveNode(parent, oldNode, out var branch))
                {
                    result = self.ReplaceChildNode(branchNode, branch);
                    return true;
                }
            }

            result = null;
            return false;
        }

        private static bool TryReplaceNode<T>(this T self, ITree parent, INode oldNode, INode newNode, out T result)
            where T : class, ITree
        {
            if (self == parent)
            {
                result = self.ReplaceChildNode(oldNode, newNode);
                return true;
            }

            foreach (var branchNode in self.GetSubTrees())
            {
                if (branchNode.TryReplaceNode(parent, oldNode, newNode, out var branch))
                {
                    result = self.ReplaceChildNode(branchNode, branch);
                    return true;
                }
            }

            result = null;
            return false;
        }

        #endregion
    }
}
