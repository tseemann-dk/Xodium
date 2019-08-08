namespace Sidekick.Models
{
    public interface IShortcut : IArchiveNode
    {
        IElement Target { get; }
    }
}
