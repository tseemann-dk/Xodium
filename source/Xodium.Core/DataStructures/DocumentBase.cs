using System.Collections.Generic;

namespace Xodium.DataStructures
{
    public abstract class DocumentBase : IDocument
    {
        protected DocumentBase(string id, string name, ITree content)
        {
            Id = id ?? throw new System.ArgumentNullException(nameof(id));
            Name = name;
            Content = content;
        }

        public string Id { get; }
        public string Name { get; }
        public ITree Content { get; }

        IReadOnlyList<INode> ITree.Nodes => Content?.Nodes;

        public abstract IDocument WithContent(ITree content);

        INode INode.Clone() => WithContent(Content);
        ITree ITree.WithNodes(IReadOnlyList<INode> nodes) => WithContent(Content?.WithNodes(nodes));
    }
}
