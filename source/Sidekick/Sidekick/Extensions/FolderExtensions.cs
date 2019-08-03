using Sidekick.Models;

namespace Sidekick.Extensions
{
    public static class FolderExtensions
    {
        public static Folder WithText(this IFolder self, string text) 
            => new Folder(self.Id, text, self.Quantity, self.Nodes);
    }
}
