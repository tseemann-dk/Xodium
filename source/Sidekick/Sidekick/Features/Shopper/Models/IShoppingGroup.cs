using System.Collections.Generic;
using System.Linq;
using Xodium.Productivity.Content.Models;

namespace Sidekick.Features.Shopper.Models
{
    public interface IShoppingGroup : IContainer, IShoppingNode
    {
        string GroupNumber { get; }
        string Title { get; }

        new IShoppingGroup WithNodes(IEnumerable<INode> nodes);
    }

    public static class ShoppingGroupExtensions
    {
        public static IEnumerable<IShoppingGroup> GetSubgroups(this IShoppingGroup self)
            => self.GetContainers().OfType<IShoppingGroup>();

        public static ShoppingGroup WithTitle(this IShoppingGroup self, string title)
            => new ShoppingGroup(self.Id, self.GroupNumber, title, self.Quantity, self.Nodes);
    }
}
