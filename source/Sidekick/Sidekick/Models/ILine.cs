namespace Sidekick.Models
{
    public interface ILine : IQuantitativeNode
    {
        IElement Element { get; }
    }
}
