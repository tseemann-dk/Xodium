using Xodium.Productivity.Content.Models;

namespace Sidekick.Models
{
    public interface IArchive : IDocument
    {
        IArchive Clone(IFolder content);
        new IFolder Content { get; }
    }
}
