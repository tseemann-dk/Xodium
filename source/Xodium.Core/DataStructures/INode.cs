using System.Collections.Generic;
using System.Linq;

namespace Xodium.DataStructures
{
    public interface INode
    {
        string Id { get; }
        INode Clone();
    }

    public static class NodeExtensions
    {
        public static string GetPath(this INode node, IContainerNode root)
        {
            var parents = node.GetAncestors(root).ToList();

            return string.Join("/", parents.Select(x => x.Id));
        }

        public static IContainerNode GetParent(this INode node, IContainerNode root)
        {
            return GetAncestors(node, root).LastOrDefault();
        }

        public static IEnumerable<IContainerNode> GetAncestors(this INode node, IContainerNode root)
        {
            var ancestors = new Stack<IContainerNode>();

            return GetAncestors(node, root, ancestors) ? ancestors.Reverse() : Enumerable.Empty<IContainerNode>();
        }

        private static bool GetAncestors(INode node, IContainerNode root, Stack<IContainerNode> ancestors)
        {
            ancestors.Push(root);

            if (node.IsChildOf(root))
                return true;

            foreach (var parent in root.Nodes.OfType<IContainerNode>())
            {
                if (GetAncestors(node, parent, ancestors))
                {
                    return true;
                }
            }

            ancestors.Pop();
            return false;
        }

        public static bool IsChildOf(this INode node, IContainerNode branch)
            => branch.Nodes.Contains(node);

        public static bool IsFirstChildOf(this INode node, IContainerNode branch)
            => node != null && branch.Nodes.FirstOrDefault() == node;

        public static bool IsLastChildOf(this INode node, IContainerNode branch)
            => node != null && branch.Nodes.LastOrDefault() == node;
    }
}
