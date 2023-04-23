using System.Collections.Generic;

namespace Xodium.DataStructures
{
    public abstract class DocumentBase : IDocument
    {
        protected DocumentBase(string id, string name, IContainerNode content)
        {
            Id = id ?? throw new System.ArgumentNullException(nameof(id));
            Name = name;
            Content = content;
        }

        public string Id { get; }
        public string Name { get; }
        public IContainerNode Content { get; }

        IReadOnlyList<INode> IContainerNode.Nodes => Content?.Nodes;

        public abstract IDocument WithContent(IContainerNode content);

        INode INode.Clone() => WithContent(Content);
        IContainerNode IContainerNode.WithNodes(IReadOnlyList<INode> nodes) => WithContent(Content?.WithNodes(nodes));
    }
}
