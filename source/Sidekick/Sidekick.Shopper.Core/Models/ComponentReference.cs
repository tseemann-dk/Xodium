using System;

namespace Sidekick.Shopper.Models
{
    public class ComponentReference : IComponentReference
    {
        public ComponentReference(ShopIdentity shop, string componentId, string componentNumber)
        {
            Shop = shop;
            ComponentId = componentId ?? throw new ArgumentNullException(nameof(componentId));
            ComponentNumber = componentNumber ?? throw new ArgumentNullException(nameof(componentNumber));
        }

        public static ComponentReference Create(string shopId, string componentId, string componentNumber) =>
            Create(ShopIdentity.Create(shopId), componentId, componentNumber);

        public static ComponentReference Create(ShopIdentity shopIdentity, string componentId, string componentNumber) =>
            new ComponentReference(shopIdentity, componentId, componentNumber);

        public ShopIdentity Shop { get; }
        public string ComponentId { get; }
        public string ComponentNumber { get; }
    }
}
