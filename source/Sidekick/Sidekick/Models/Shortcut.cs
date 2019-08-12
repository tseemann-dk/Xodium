using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Xodium.Productivity.Content.Models;

namespace Sidekick.Models
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class Shortcut : IShortcut
    {
        public Shortcut(IElement element, double quantity, string text = null, double? value = null)
            : this(Guid.NewGuid().ToString(), element, quantity, text, value)
        {
        }

        public Shortcut(string id, IElement element, double quantity, string text = null, double? value = null)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Element = element ?? throw new ArgumentNullException(nameof(element));
            ElementNumber = element?.Number;
            Quantity = quantity;
            Text = text;
            Value = value;
        }

        public string Id { get; }
        public string ElementNumber { get; }
        public IElement Element { get; }
        public string Text { get; }
        public double Quantity { get; }
        public double? Value { get; }

        private string DisplayNumber => Element?.Number;
        private string DisplayText => Text ?? Element?.Text;

        string IArchiveNode.ReferenceNumber => DisplayNumber;
        string IArchiveNode.Text => DisplayText;
        double IArchiveNode.Value => Value ?? Element?.Value ?? 0;

        [ExcludeFromCodeCoverage]
        private string DebuggerDisplay => $"{DisplayNumber}, {DisplayText}";

        public INode Clone() => new Shortcut(Id, Element, Quantity, Text, Value);
    }
}
