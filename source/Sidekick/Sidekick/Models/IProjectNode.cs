using System;
using Xodium.Productivity.Content.Models;

namespace Sidekick.Models
{
    public interface IProjectNode : INode
    {
        string ReferenceNumber { get; }
        string Text { get; }
        double Quantity { get; }
        double Value { get; }
    }
}
