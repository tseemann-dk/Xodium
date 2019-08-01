using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Xodium.Productivity.Content.Models;

namespace Sidekick.Models
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class Project : DocumentBase, IProject
    {
        public Project(string id, string name, IFolder root)
            : base(id, name, root)
        {
        }

        public new IFolder Content => base.Content as IFolder;

        [ExcludeFromCodeCoverage]
        private string DebuggerDisplay => $"{Name}; {Content.Number}, {Content.Text}";

        public IProject Clone(IFolder content) => new Project(Id, Name, content);
        public override IDocument Clone(IContainer content) => Clone(content as IFolder);
    }
}
