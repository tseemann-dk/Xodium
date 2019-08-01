using Xodium.Productivity.Content.Models;

namespace Sidekick.Models
{
    public interface IProject : IDocument
    {
        IProject Clone(IFolder content);
        new IFolder Content { get; }
    }
}
