using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Xodium.Productivity.Content.Models;

namespace Sidekick.Features.Shopper.Models
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class ShoppingList : DocumentBase, IShoppingList
    {
        private readonly IReadOnlyList<IComponent> components;

        public ShoppingList(string id, string name, IShoppingGroup root, IReadOnlyList<IComponent> components = null)
            : base(id, name, root)
        {
            this.components = components ?? new List<IComponent>();
        }

        public new IShoppingGroup Content => base.Content as IShoppingGroup;
        public IReadOnlyList<IComponent> Components => components;

        [ExcludeFromCodeCoverage]
        private string DebuggerDisplay => $"{Name}; {Content.Title}";

        public ShoppingList AddComponent(IComponent component)
        {
            var list = Components.ToList();
            list.Add(component);
            return WithComponents(list);
        }

        public ShoppingList AddNode(
            IShoppingGroup parentGroup,
            INode node,
            string afterNodeId = null)
        {
            IShoppingGroup newParentGroup;

            if (afterNodeId == null)
            {
                newParentGroup = parentGroup.InsertNode(0, node);
            }
            else
            {
                var afterNode = parentGroup.Nodes.FirstOrDefault(x => x.Id == afterNodeId);
                var index = parentGroup.GetIndexOfNode(afterNode);

                newParentGroup = index >= parentGroup.Nodes.Count
                    ? parentGroup.AddNode(node)
                    : parentGroup.InsertNode(index + 1, node);
            }

            return ReplaceNode(parentGroup, newParentGroup);
        }

        public ShoppingList ChangeGroupTitle(IShoppingGroup group, string newTitle)
        {
            return ReplaceNode(group, group.WithTitle(newTitle));
        }

        public ShoppingList DeleteNode(IShoppingGroup parentGroup, string nodeId)
        {
            return DeleteNode(parentGroup, parentGroup.GetChildNode(nodeId) as IShoppingNode);
        }

        public ShoppingList DeleteNode(IShoppingGroup parentGroup, IShoppingNode node)
        {
            return ReplaceNode(parentGroup, parentGroup.RemoveNode(node));
        }

        public ShoppingList MoveNodeDown(IShoppingGroup parentGroup, string nodeId)
        {
            return MoveNodeDown(parentGroup, parentGroup.GetChildNode(nodeId) as IShoppingNode);
        }

        public ShoppingList MoveNodeDown(IShoppingGroup parentGroup, IShoppingNode node)
        {
            var nextNode = parentGroup.GetNextNode(node);
            if (nextNode == null) return this;
            return ReplaceNode(parentGroup, parentGroup.SwapChildNodes(node, nextNode));
        }

        public ShoppingList MoveNodeUp(IShoppingGroup parentGroup, string nodeId)
        {
            return MoveNodeUp(parentGroup, parentGroup.GetChildNode(nodeId) as IShoppingNode);
        }

        public ShoppingList MoveNodeUp(IShoppingGroup parentGroup, IShoppingNode node)
        {
            var previousNode = parentGroup.GetPreviousNode(node);
            if (previousNode == null) return this;
            return ReplaceNode(parentGroup, parentGroup.SwapChildNodes(node, previousNode));
        }

        public ShoppingList RemoveComponent(IComponent component)
        {
            var newComponents = Components.ToList();
            newComponents.Remove(component);
            return WithComponents(newComponents);
        }

        public ShoppingList ReplaceNode(IShoppingNode oldNode, IShoppingNode newNode)
        {
            if (oldNode is IShoppingGroup oldGroup && oldGroup.Id == Content.Id && newNode is IShoppingGroup newGroup)
                return WithContent(newGroup);

            return WithContent(Content.ReplaceNode(oldNode, newNode));
        }

        public ShoppingList WithContent(IShoppingGroup content) => new ShoppingList(Id, Name, content, Components);
        public ShoppingList WithComponents(IReadOnlyList<IComponent> components) => new ShoppingList(Id, Name, Content, components);
        public override IDocument WithContent(IContainer content) => WithContent(content as IShoppingGroup);

        IShoppingList IShoppingList.AddComponent(IComponent component) => AddComponent(component);
        IShoppingList IShoppingList.RemoveComponent(IComponent component) => RemoveComponent(component);
        IShoppingList IShoppingList.WithContent(IShoppingGroup content) => WithContent(content);
        IShoppingList IShoppingList.WithComponents(IReadOnlyList<IComponent> components) => WithComponents(components);
    }
}
