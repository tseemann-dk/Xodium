namespace Sidekick.Features.Shopper.Models
{
    public class ComponentDescriptor : IComponentDescriptor
    {
        public ComponentDescriptor(IComponentReference reference, string text, double price)
        {
            Reference = reference ?? throw new System.ArgumentNullException(nameof(reference));
            Text = text;
            Price = price;
        }

        public IComponentReference Reference { get; }
        public string Text { get; }
        public double Price { get; }
    }
}
