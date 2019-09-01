namespace Sidekick.Shopper.Models
{
    public interface IComponentDescriptor
    {
        IComponentReference Reference { get; }
        string Text { get; }
        string ThumbnailUrl { get; } 
        double Price { get; }
    }
}
