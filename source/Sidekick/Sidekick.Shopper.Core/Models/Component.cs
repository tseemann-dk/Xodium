using System;

namespace Sidekick.Shopper.Models
{
    public class Component : IComponent
    {
        public Component(ShopIdentity origin, string componentNumber, string text, string thumbnailUrl, double price)
        {
            Origin = origin;
            ComponentNumber = componentNumber ?? throw new ArgumentNullException(nameof(componentNumber));
            Text = text ?? throw new ArgumentNullException(nameof(text));
            ThumbnailUrl = thumbnailUrl;
            Price = price;
        }

        public ShopIdentity Origin { get; }
        public string ComponentNumber { get; }
        public string Text { get; }
        public string ThumbnailUrl { get; }
        public double Price { get; }
    }
}
