namespace Sidekick.Shopper.Models
{
    public interface IComponent
    {
        ShopIdentity Origin { get; }
        string ComponentNumber { get; }
        string Text { get; }
        double Price { get; }
    }

    public static class ComponentExtensions
    {
        public static bool EqualsReference(this IComponent self, IComponentReference reference) =>
            self.Origin.Equals(reference.Shop) && self.ComponentNumber == reference.ComponentNumber;
    }
}
