using System;
using System.Collections.Generic;
using System.Linq;

namespace Xodium.Productivity.Content.Models
{
    public interface IContainer : IBranch
    {
        IContainer Clone(IEnumerable<INode> nodes);
    }

    public static class ContainerExtensions
    {
        public static IEnumerable<IContainer> GetContainers(this IContainer self) 
            => self.Nodes.OfType<IContainer>();

        public static T AddNode<T>(this T self, INode node)
            where T : class, IContainer
            => self.AddNodes(new[] { node });

        public static T AddNodeAt<T>(this T self, IContainer parent, INode node)
            where T : class, IContainer
            => self.AddNodesAt(parent, new[] { node });

        public static T AddNodes<T>(this T self, IEnumerable<INode> nodes)
            where T : class, IContainer 
            => self.Clone(self.Nodes.Concat(nodes)) as T;

        public static T AddNodesAt<T>(this T self, IContainer parent, IEnumerable<INode> nodes)
            where T : class, IContainer
        {
            if (self.TryAddNodes(parent, nodes, out var result))
            {
                return result as T;
            }

            throw new ArgumentException("Not found", nameof(parent));
        }

        public static T InsertNode<T>(this T self, int index, INode node)
            where T : class, IContainer
            => self.InsertNodes(index, new[] { node });

        public static T InsertNodeAt<T>(this T self, IContainer parent, int index, INode node)
            where T : class, IContainer
            => self.InsertNodesAt(parent, index, new[] { node });

        public static T InsertNodes<T>(this T self, int index, IEnumerable<INode> nodes)
            where T : class, IContainer
        {
            var newNodes = self.Nodes.ToList();
            newNodes.InsertRange(index, nodes);
            return self.Clone(newNodes) as T;
        }

        public static T InsertNodesAt<T>(this T self, IContainer parent, int index, IEnumerable<INode> nodes)
            where T : class, IContainer
        {
            if (self.TryInsertNodes(parent, index, nodes, out var result))
            {
                return result as T;
            }

            throw new ArgumentException("Not found", nameof(parent));
        }

        public static T RemoveNode<T>(this T self, INode oldNode)
            where T : class, IContainer
        {
            if (oldNode.IsChildOf(self))
            {
                return self.RemoveChildNode(oldNode);
            }

            if (oldNode.GetParent(self) is IContainer parent)
            {
                return self.RemoveNodeAt(parent, oldNode);
            }

            throw new ArgumentException("Not found", nameof(oldNode));
        }

        private static T RemoveChildNode<T>(this T self, INode oldNode)
            where T : class, IContainer
        {
            var nodes = self.Nodes.ToList();
            nodes.Remove(oldNode);
            return self.Clone(nodes) as T;
        }

        public static T RemoveNodeAt<T>(this T self, IContainer parent, INode oldNode)
            where T : class, IContainer
        {
            if (self.TryRemoveNode(parent, oldNode, out var result))
            {
                return result as T;
            }

            throw new ArgumentException("Not found", nameof(oldNode));
        }

        public static T RemoveChildNodes<T>(this T self, IEnumerable<INode> oldNodes)
            where T : class, IContainer
        {
            var nodes = self.Nodes.ToList();
            nodes.RemoveAll(x => oldNodes.Contains(x));
            return self.Clone(nodes) as T;
        }

        public static T ReplaceNode<T>(this T self, INode oldNode, INode newNode)
            where T : class, IContainer
        {
            if (self.Nodes.Contains(oldNode))
            {
                return self.ReplaceChildNode(oldNode, newNode);
            }

            if (oldNode.GetParent(self) is IContainer parent)
            {
                return self.ReplaceNodeAt(parent, oldNode, newNode);
            }

            throw new ArgumentException("Not found", nameof(oldNode));
        }

        private static T ReplaceChildNode<T>(this T self, INode oldNode, INode newNode)
            where T : class, IContainer
        {
            var nodes = self.Nodes.ToList();
            var index = nodes.IndexOf(oldNode);

            if (index < 0)
                throw new ArgumentException("Not found", nameof(oldNode));

            nodes.Insert(index, newNode);
            nodes.Remove(oldNode);

            return self.Clone(nodes) as T;
        }

        public static T ReplaceNodeAt<T>(this T self, IContainer parent, INode oldNode, INode newNode)
            where T : class, IContainer
        {
            if (self.TryReplaceNode(parent, oldNode, newNode, out var result))
            {
                return result as T;
            }

            throw new ArgumentException("Not found", nameof(oldNode));
        }

        public static T SwapChildNodes<T>(this T self, INode node1, INode node2)
            where T : class, IContainer
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

            return self.Clone(nodes) as T;
        }

        public static bool TryAddNodes<T>(this T self, IContainer parent, IEnumerable<INode> nodes, out T result)
            where T : class, IContainer
        {
            if (self == parent)
            {
                result = self.AddNodes(nodes);
                return true;
            }

            foreach (var container in self.GetContainers())
            {
                if (container.TryAddNodes(parent, nodes, out var branch))
                {
                    result = self.ReplaceChildNode(container, branch);
                    return true;
                }
            }

            result = null;
            return false;
        }

        public static bool TryInsertNodes<T>(this T self, IContainer parent, int index, IEnumerable<INode> nodes, out T result)
            where T : class, IContainer
        {
            if (self == parent)
            {
                result = self.InsertNodes(index, nodes);
                return true;
            }

            foreach (var container in self.GetContainers())
            {
                if (container.TryInsertNodes(parent, index, nodes, out var branch))
                {
                    result = self.ReplaceChildNode(container, branch);
                    return true;
                }
            }

            result = null;
            return false;
        }

        public static bool TryRemoveNode<T>(this T self, IContainer parent, INode oldNode, out T result)
            where T : class, IContainer
        {
            if (self == parent)
            {
                result = self.RemoveChildNode(oldNode);
                return true;
            }

            foreach (var container in self.GetContainers())
            {
                if (container.TryRemoveNode(parent, oldNode, out var branch))
                {
                    result = self.ReplaceChildNode(container, branch);
                    return true;
                }
            }

            result = null;
            return false;
        }

        public static bool TryReplaceNode<T>(this T self, IContainer parent, INode oldNode, INode newNode, out T result)
            where T : class, IContainer
        {
            if (self == parent)
            {
                result = self.ReplaceChildNode(oldNode, newNode);
                return true;
            }

            foreach (var container in self.GetContainers())
            {
                if (container.TryReplaceNode(parent, oldNode, newNode, out var branch))
                {
                    result = self.ReplaceChildNode(container, branch);
                    return true;
                }
            }

            result = null;
            return false;
        }
    }
}
