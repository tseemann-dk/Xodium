using Xodium.Productivity.Content.Models;

namespace Sidekick.Models
{
    public interface IProjectNode : INode
    {
        string Text { get; }
        double Quantity { get; }
        double Value { get; }
    }
}
