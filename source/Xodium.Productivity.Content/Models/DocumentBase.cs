using System.Collections.Generic;

namespace Xodium.Productivity.Content.Models
{
    public abstract class DocumentBase : IDocument
    {
        public DocumentBase(string id, string name, IContainer content)
        {
            Id = id ?? throw new System.ArgumentNullException(nameof(id));
            Name = name;
            Content = content;
        }

        public string Id { get; }
        public string Name { get; }
        public IContainer Content { get; }

        IReadOnlyList<INode> IBranch.Nodes => Content?.Nodes;

        public abstract IDocument WithContent(IContainer content);

        INode INode.Clone() => WithContent(Content);
        IContainer IContainer.WithNodes(IEnumerable<INode> nodes) => WithContent(Content?.WithNodes(nodes));
    }
}
