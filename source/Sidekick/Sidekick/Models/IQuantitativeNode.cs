using Xodium.Productivity.Content.Models;

namespace Sidekick.Models
{
    public interface IQuantitativeNode : INode
    {
        string Number { get; }
        string Text { get; }
        double Quantity { get; }
    }
}
