namespace Sidekick.Features.Shopper.Models
{
    public interface IComponent
    {
        string ComponentNumber { get; }
        string Text { get; }
        double Price { get; }
    }
}
