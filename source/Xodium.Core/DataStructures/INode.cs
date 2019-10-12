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
        public static string GetPath(this INode node, ITree root)
        {
            var parents = node.GetAncestors(root).ToList();

            return string.Join("/", parents.Select(x => x.Id));
        }

        public static ITree GetParent(this INode node, ITree root)
        {
            return GetAncestors(node, root).LastOrDefault();
        }

        public static IEnumerable<ITree> GetAncestors(this INode node, ITree root)
        {
            var ancestors = new Stack<ITree>();

            return GetAncestors(node, root, ancestors) ? ancestors.Reverse() : Enumerable.Empty<ITree>();
        }

        private static bool GetAncestors(INode node, ITree root, Stack<ITree> ancestors)
        {
            ancestors.Push(root);

            if (node.IsChildOf(root))
                return true;

            foreach (var parent in root.Nodes.OfType<ITree>())
            {
                if (GetAncestors(node, parent, ancestors))
                {
                    return true;
                }
            }

            ancestors.Pop();
            return false;
        }

        public static bool IsChildOf(this INode node, ITree branch)
            => branch.Nodes.Contains(node);

        public static bool IsFirstChildOf(this INode node, ITree branch)
            => node != null && branch.Nodes.FirstOrDefault() == node;

        public static bool IsLastChildOf(this INode node, ITree branch)
            => node != null && branch.Nodes.LastOrDefault() == node;
    }
}
