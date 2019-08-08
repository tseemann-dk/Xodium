using Xodium.Productivity.Content.Models;

namespace Sidekick.Models
{
    public interface IArchiveNode : INode
    {
        string ReferenceNumber { get; }
        string Text { get; }
        double Quantity { get; }
        double Value { get; }
    }
}
