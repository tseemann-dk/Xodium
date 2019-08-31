using System.Collections.Generic;
using System.Linq;
using Xodium.Productivity.Content.Models;

namespace Sidekick.Shopper.Models
{
    public interface IShoppingGroup : IContainer, IShoppingNode
    {
        string GroupNumber { get; }
        string Title { get; }

        IShoppingGroup WithTitle(string title);
        new IShoppingGroup WithNodes(IReadOnlyList<INode> nodes);
    }

    public static class ShoppingGroupExtensions
    {
        public static IEnumerable<IShoppingGroup> GetSubgroups(this IShoppingGroup self)
            => self.GetContainers().OfType<IShoppingGroup>();
    }
}
