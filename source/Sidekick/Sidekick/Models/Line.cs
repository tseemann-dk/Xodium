using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Xodium.Productivity.Content.Models;

namespace Sidekick.Models
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class Line : ILine
    {

        public Line(IElement element, double quantity = 1)
            : this(Guid.NewGuid().ToString(), element, quantity)
        {
        }

        public Line(string id, IElement element, double quantity = 1)
        {
            Id = id;
            Element = element;
            Quantity = quantity;
        }

        public string Id { get; }
        public string Number => Element?.Number;
        public string Text => Element?.Text;
        public double Quantity { get; }
        public IElement Element { get; }

        [ExcludeFromCodeCoverage]
        private string DebuggerDisplay => $"{Number}, {Text}";

        public INode Clone() => new Line(Id, Element, Quantity);
    }
}
