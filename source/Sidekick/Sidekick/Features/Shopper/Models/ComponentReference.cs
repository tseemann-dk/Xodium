using System;

namespace Sidekick.Features.Shopper.Models
{
    public class ComponentReference : IComponentReference
    {
        public ComponentReference(ShopIdentity shop, string componentNumber)
        {
            Shop = shop;
            ComponentNumber = componentNumber ?? throw new ArgumentNullException(nameof(componentNumber));
        }

        public ShopIdentity Shop { get; }
        public string ComponentNumber { get; }
    }
}
