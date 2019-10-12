namespace Xodium.Productivity.Content.Models
{
    public interface IDocument : ITree
    {
        string Name { get; }
        ITree Content { get; }
    }
}
