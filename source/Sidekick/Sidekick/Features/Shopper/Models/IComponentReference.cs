namespace Sidekick.Features.Shopper.Models
{
    public interface IComponentReference
    {
        ShopIdentity Shop { get; }
        string ComponentNumber { get; }
    }
}
