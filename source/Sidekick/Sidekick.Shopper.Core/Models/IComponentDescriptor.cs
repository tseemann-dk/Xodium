namespace Sidekick.Shopper.Models
{
    public interface IComponentDescriptor
    {
        IComponentReference Reference { get; }
        string Text { get; }
        double Price { get; }
    }
}
