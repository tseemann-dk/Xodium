using System.Collections.Generic;
using System.Linq;
using Xodium.DataStructures;

namespace Sidekick.Shopper.Models
{
    public interface IShoppingFolder : ITree, IShoppingNode
    {
        string FolderNumber { get; }
        string Title { get; }

        IShoppingFolder WithTitle(string title);
        new IShoppingFolder WithNodes(IReadOnlyList<INode> nodes);
    }

    public static class ShoppingFolderExtensions
    {
        public static IEnumerable<IShoppingFolder> GetSubfolders(this IShoppingFolder self)
            => self.GetSubTrees().OfType<IShoppingFolder>();
    }
}
