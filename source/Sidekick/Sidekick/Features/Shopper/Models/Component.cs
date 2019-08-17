using System;

namespace Sidekick.Features.Shopper.Models
{
    public class Component : IComponent
    {
        public Component(string componentNumber, string text, double price)
        {
            ComponentNumber = componentNumber ?? throw new ArgumentNullException(nameof(componentNumber));
            Text = text ?? throw new ArgumentNullException(nameof(text));
            Price = price;
        }

        public string ComponentNumber { get; }
        public string Text { get; }
        public double Price { get; }
    }
}
