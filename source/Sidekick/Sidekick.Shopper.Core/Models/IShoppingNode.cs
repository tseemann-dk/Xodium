using Xodium.Productivity.Content.Models;

namespace Sidekick.Shopper.Models
{
    public interface IShoppingNode : INode
    {
        string ReferenceNumber { get; }
        string Text { get; }
        double Quantity { get; }
        double Price { get; }
    }
}
