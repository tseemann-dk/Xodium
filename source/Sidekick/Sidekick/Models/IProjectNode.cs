using System;
using Xodium.Productivity.Content.Models;

namespace Sidekick.Models
{
    public interface IProjectNode : INode
    {
        DateTime? Date { get; }
        string Text { get; }
        double Quantity { get; }
        double Value { get; }
    }
}
