using System;

namespace Sidekick.Models
{
    public interface ILine : IExpenseNode
    {
        DateTime Date { get; }
    }
}
