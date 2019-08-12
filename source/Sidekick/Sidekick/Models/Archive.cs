using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Xodium.Productivity.Content.Models;

namespace Sidekick.Models
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class Archive : DocumentBase, IArchive
    {
        private readonly List<IElement> elements;

        public Archive(string id, string name, IFolder root, IEnumerable<IElement> elements = null)
            : base(id, name, root)
        {
            this.elements = elements?.ToList() ?? new List<IElement>();
        }

        public new IFolder Content => base.Content as IFolder;
        public IReadOnlyList<IElement> Elements => elements;

        [ExcludeFromCodeCoverage]
        private string DebuggerDisplay => $"{Name}; {Content.Text}";

        public IArchive AddElement(IElement element)
        {
            var list = Elements.ToList();
            list.Add(element);
            return WithElements(list);
        }

        public IArchive RemoveElement(IElement element)
        {
            var list = Elements.ToList();
            list.Remove(element);
            return WithElements(list);
        }

        public IArchive WithContent(IFolder content) => new Archive(Id, Name, content, Elements);
        public IArchive WithElements(IEnumerable<IElement> elements) => new Archive(Id, Name, Content, elements);

        public override IDocument WithContent(IContainer content) => WithContent(content as IFolder);
    }
}
