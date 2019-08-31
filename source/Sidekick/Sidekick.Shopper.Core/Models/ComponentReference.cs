using System;

namespace Sidekick.Shopper.Models
{
    public class ComponentReference : IComponentReference
    {
        public ComponentReference(ShopIdentity shop, string componentNumber)
        {
            Shop = shop;
            ComponentNumber = componentNumber ?? throw new ArgumentNullException(nameof(componentNumber));
        }

        public static ComponentReference Create(string shopId, string componentNumber) =>
            Create(ShopIdentity.Create(shopId), componentNumber);

        public static ComponentReference Create(ShopIdentity shopIdentity, string componentNumber) =>
            new ComponentReference(shopIdentity, componentNumber);

        public ShopIdentity Shop { get; }
        public string ComponentNumber { get; }
    }
}
