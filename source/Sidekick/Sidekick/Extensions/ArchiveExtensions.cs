using Sidekick.Models;

namespace Sidekick.Extensions
{
    public static class ArchiveExtensions
    {
        public static Archive WithText(this IArchive self, string text)
            => new Archive(self.Id, self.Name, self.Content.WithText(text), self.Elements);
    }
}
