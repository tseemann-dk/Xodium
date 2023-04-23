namespace Xodium.DataStructures
{
    public interface IDocument : IContainerNode
    {
        string Name { get; }
        IContainerNode Content { get; }
    }
}
