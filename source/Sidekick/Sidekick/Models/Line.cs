using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Xodium.Productivity.Content.Models;

namespace Sidekick.Models
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class Line : ILine
    {
        public Line(DateTime date, string text, double quantity, double value)
            : this(Guid.NewGuid().ToString(), date, text, quantity, value)
        {
        }

        public Line(string id, DateTime date, string text, double quantity, double value)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Date = date;
            Text = text;
            Quantity = quantity;
            Value = value;
        }

        public string Id { get; }
        public DateTime Date { get; }
        public string Text { get; }
        public double Quantity { get; }
        public double Value { get; }

        string IProjectNode.ReferenceNumber => Date.ToString("dd-MM-yy");

        [ExcludeFromCodeCoverage]
        private string DebuggerDisplay => $"{Date}, {Text}";

        public INode Clone() => new Line(Id, Date, Text, Quantity, Value);
    }
}
