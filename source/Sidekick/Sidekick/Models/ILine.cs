using System;

namespace Sidekick.Models
{
    public interface ILine : IProjectNode
    {
        DateTime Date { get; }
    }
}
