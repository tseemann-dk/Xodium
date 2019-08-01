using Sidekick.Models;

namespace Sidekick.Extensions
{
    public static class ProjectExtensions
    {
        public static Project WithText(this IProject self, string text)
            => new Project(self.Id, self.Name, self.Content.WithText(text));
    }
}
