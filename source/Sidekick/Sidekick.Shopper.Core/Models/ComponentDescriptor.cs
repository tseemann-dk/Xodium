namespace Sidekick.Shopper.Models
{
    public class ComponentDescriptor : IComponentDescriptor
    {
        public ComponentDescriptor(IComponentReference reference, string text, double price)
        {
            Reference = reference ?? throw new System.ArgumentNullException(nameof(reference));
            Text = text;
            Price = price;
        }

        public static ComponentDescriptor Create(
            string shopId, string componentNumber,
            string text, double price = 0) 
            =>
            new ComponentDescriptor(
                ComponentReference.Create(shopId, componentNumber),
                text, price
            );

        public IComponentReference Reference { get; }
        public string Text { get; }
        public double Price { get; }
    }
}
