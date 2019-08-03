using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Xodium.Productivity.Content.Models;

namespace Sidekick.Models
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class ExpenseDocument : DocumentBase, IExpenseDocument
    {
        public ExpenseDocument(string id, string name, IFolder root)
            : base(id, name, root)
        {
        }

        public new IFolder Content => base.Content as IFolder;

        [ExcludeFromCodeCoverage]
        private string DebuggerDisplay => $"{Name}; {Content.Text}";

        public IExpenseDocument Clone(IFolder content) => new ExpenseDocument(Id, Name, content);
        public override IDocument Clone(IContainer content) => Clone(content as IFolder);
    }
}
