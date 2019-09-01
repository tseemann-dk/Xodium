namespace Sidekick.Shopper.Models
{
    public interface IComponentReference
    {
        ShopIdentity Shop { get; }
        string ComponentId { get; }
        string ComponentNumber { get; }
    }
}
