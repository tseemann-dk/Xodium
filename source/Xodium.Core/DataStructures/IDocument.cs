namespace Xodium.DataStructures
{
    public interface IDocument : ITree
    {
        string Name { get; }
        ITree Content { get; }
    }
}
