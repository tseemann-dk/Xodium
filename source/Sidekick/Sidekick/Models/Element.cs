using System;

namespace Sidekick.Models
{
    public class Element : IElement
    {
        public Element(string number, string text, double value)
        {
            Number = number ?? throw new ArgumentNullException(nameof(number));
            Text = text ?? throw new ArgumentNullException(nameof(text));
            Value = value;
        }

        public string Number { get; }
        public string Text { get; }
        public double Value { get; }
    }
}
