using Xodium.Productivity.Content.Models;

namespace Sidekick.Models
{
    public interface IExpenseNode : INode
    {
        string Text { get; }
        double Quantity { get; }
        double Value { get; }
    }
}
