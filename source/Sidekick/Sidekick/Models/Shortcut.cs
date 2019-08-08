using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Xodium.Productivity.Content.Models;

namespace Sidekick.Models
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class Shortcut : IShortcut
    {
        public Shortcut(IElement target, double quantity, string text = null, double? value = null)
            : this(Guid.NewGuid().ToString(), target, quantity, text, value)
        {
        }

        public Shortcut(string id, IElement target, double quantity, string text = null, double? value = null)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Target = target;
            Quantity = quantity;
            Text = text;
            Value = value;
        }

        public string Id { get; }
        public IElement Target { get; }
        public string Text { get; }
        public double Quantity { get; }
        public double? Value { get; }

        private string DisplayNumber => Target?.Number;
        private string DisplayText => Text ?? Target?.Text;

        string IArchiveNode.ReferenceNumber => DisplayNumber;
        string IArchiveNode.Text => DisplayText;
        double IArchiveNode.Value => Value ?? Target?.Value ?? 0;

        [ExcludeFromCodeCoverage]
        private string DebuggerDisplay => $"{DisplayNumber}, {DisplayText}";

        public INode Clone() => new Shortcut(Id, Target, Quantity, Text, Value);
    }
}
