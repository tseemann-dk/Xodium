using Xodium.Productivity.Content.Models;

namespace Sidekick.Models
{
    public interface IExpenseDocument : IDocument
    {
        IExpenseDocument Clone(IFolder content);
        new IFolder Content { get; }
    }
}
