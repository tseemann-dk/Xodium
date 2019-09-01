namespace Sidekick.Shopper.Models
{
    public class ComponentDescriptor : IComponentDescriptor
    {
        public ComponentDescriptor(IComponentReference reference, string text, string thumbnailUrl, double price)
        {
            Reference = reference ?? throw new System.ArgumentNullException(nameof(reference));
            Text = text;
            ThumbnailUrl = thumbnailUrl;
            Price = price;
        }

        public static ComponentDescriptor Create(
            string shopId, string componentId, string componentNumber,
            string text, string thumbnailUrl = null,  double price = 0) 
            =>
            new ComponentDescriptor(
                ComponentReference.Create(shopId, componentId, componentNumber),
                text, thumbnailUrl, price
            );

        public static ComponentDescriptor Create(
            ShopIdentity shopIdentity, string componentId, string componentNumber,
            string text, string thumbnailUrl = null, double price = 0)
            =>
            new ComponentDescriptor(
                ComponentReference.Create(shopIdentity, componentId, componentNumber),
                text, thumbnailUrl, price
            );

        public IComponentReference Reference { get; }
        public string Text { get; }
        public string ThumbnailUrl { get; }
        public double Price { get; }
    }
}
