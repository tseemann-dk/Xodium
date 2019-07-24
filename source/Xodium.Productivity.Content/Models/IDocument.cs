namespace Xodium.Productivity.Content.Models
{
    public interface IDocument : IContainer
    {
        string Name { get; }
        IContainer Content { get; }
    }
}
