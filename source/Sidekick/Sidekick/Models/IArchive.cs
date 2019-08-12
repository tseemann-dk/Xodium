using System.Collections.Generic;
using Xodium.Productivity.Content.Models;

namespace Sidekick.Models
{
    public interface IArchive : IDocument
    {
        new IFolder Content { get; }
        IReadOnlyList<IElement> Elements { get; }

        IArchive AddElement(IElement element);
        IArchive RemoveElement(IElement element);
        IArchive WithContent(IFolder content);
        IArchive WithElements(IEnumerable<IElement> elements);
    }
}
