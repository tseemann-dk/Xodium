using System;

namespace Sidekick.Models
{
    public interface IShortcut : IArchiveNode
    {
        DateTime Date { get; }
    }
}
