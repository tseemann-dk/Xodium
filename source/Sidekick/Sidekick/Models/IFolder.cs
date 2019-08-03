using System.Collections.Generic;
using System.Linq;
using Xodium.Productivity.Content.Models;

namespace Sidekick.Models
{
    public interface IFolder : IContainer, IExpenseNode
    {
        new IFolder Clone(IEnumerable<INode> nodes);
    }

    public static class FolderExtensions
    {
        public static IEnumerable<IFolder> GetSubfolders(this IFolder self)
            => self.GetContainers().OfType<IFolder>();
    }
}
