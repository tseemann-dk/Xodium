using System.Collections.Generic;
using System.Linq;

namespace Xodium.Productivity.Content.Models
{
    public interface INode
    {
        string Id { get; }
        INode Clone();
    }

    public static class NodeExtensions
    {
        public static string GetPath(this INode node, IBranch root)
        {
            var parents = node.GetParents(root).ToList();

            return string.Join("/", parents.Select(x => x.Id));
        }

        public static IBranch GetParent(this INode node, IBranch root)
        {
            return GetParents(node, root).LastOrDefault();
        }

        public static IEnumerable<IBranch> GetParents(this INode node, IBranch root)
        {
            var parents = new Stack<IBranch>();

            return GetParents(node, root, parents) ? parents.Reverse() : Enumerable.Empty<IBranch>();
        }

        private static bool GetParents(INode node, IBranch root, Stack<IBranch> parents)
        {
            parents.Push(root);

            if (node.IsChildOf(root))
                return true;

            foreach (var parent in root.Nodes.OfType<IBranch>())
            {
                if (GetParents(node, parent, parents))
                {
                    return true;
                }
            }

            parents.Pop();
            return false;
        }

        public static bool IsChildOf(this INode node, IBranch branch)
            => branch.Nodes.Contains(node);

        public static bool IsFirstChildOf(this INode node, IBranch branch)
            => node != null && branch.Nodes.FirstOrDefault() == node;

        public static bool IsLastChildOf(this INode node, IBranch branch)
            => node != null && branch.Nodes.LastOrDefault() == node;
    }
}
