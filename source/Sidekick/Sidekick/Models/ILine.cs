using System;

namespace Sidekick.Models
{
    public interface ILine : IProjectNode
    {
        new DateTime Date { get; }
    }
}
