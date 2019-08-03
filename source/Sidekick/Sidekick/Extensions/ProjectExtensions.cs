using Sidekick.Models;

namespace Sidekick.Extensions
{
    public static class ProjectExtensions
    {
        public static ExpenseDocument WithText(this IExpenseDocument self, string text)
            => new ExpenseDocument(self.Id, self.Name, self.Content.WithText(text));
    }
}
