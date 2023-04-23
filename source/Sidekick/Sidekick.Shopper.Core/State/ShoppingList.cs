using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Sidekick.Shopper.Models;
using Xodium.DataStructures;

namespace Sidekick.Shopper.State
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class ShoppingList : DocumentBase, IShoppingList
    {
        public ShoppingList(string id, string name, IShoppingFolder root, IReadOnlyList<IComponent> components = null)
            : base(id, name, root)
        {
            Components = components ?? new List<IComponent>();
        }

        public new IShoppingFolder Content => base.Content as IShoppingFolder;
        public IReadOnlyList<IComponent> Components { get; }

        [ExcludeFromCodeCoverage]
        private string DebuggerDisplay => $"{Name}; {Content.Title}";

        public ShoppingList AddComponent(IComponent component)
        {
            var list = Components.ToList();
            list.Add(component);
            return WithComponents(list);
        }

        public ShoppingList AddNode(
            IShoppingFolder folder,
            INode node,
            string afterNodeId = null)
        {
            IShoppingFolder newFolder;

            if (afterNodeId == null)
            {
                newFolder = folder.InsertNode(0, node);
            }
            else
            {
                var afterNode = folder.Nodes.FirstOrDefault(x => x.Id == afterNodeId);
                var index = folder.GetIndexOfNode(afterNode);

                newFolder = index >= folder.Nodes.Count
                    ? folder.AddNode(node)
                    : folder.InsertNode(index + 1, node);
            }

            return ReplaceNode(folder, newFolder);
        }

        public IComponent FindComponent(IComponentReference reference)
        {
            return Components.FirstOrDefault(x => x.EqualsReference(reference));
        }

        public IShoppingFolder FindFolder(string id)
        {
            return Content.FindNode<IShoppingFolder>(x => x.Id == id);
        }

        public ShoppingList ChangeFolderTitle(IShoppingFolder folder, string newTitle)
        {
            return ReplaceNode(folder, folder.WithTitle(newTitle));
        }

        public ShoppingList DeleteNode(IShoppingFolder folder, string nodeId)
        {
            return DeleteNode(folder, folder.GetChildNode(nodeId) as IShoppingNode);
        }

        public ShoppingList DeleteNode(IShoppingFolder folder, IShoppingNode node)
        {
            return ReplaceNode(folder, folder.RemoveNode(node));
        }

        public ShoppingList MoveNodeDown(IShoppingFolder folder, string nodeId)
        {
            return MoveNodeDown(folder, folder.GetChildNode(nodeId) as IShoppingNode);
        }

        public ShoppingList MoveNodeDown(IShoppingFolder folder, IShoppingNode node)
        {
            var nextNode = folder.GetNextSibling(node);
            if (nextNode == null) return this;
            return ReplaceNode(folder, folder.SwapChildNodes(node, nextNode));
        }

        public ShoppingList MoveNodeUp(IShoppingFolder folder, string nodeId)
        {
            return MoveNodeUp(folder, folder.GetChildNode(nodeId) as IShoppingNode);
        }

        public ShoppingList MoveNodeUp(IShoppingFolder folder, IShoppingNode node)
        {
            var previousNode = folder.GetPreviousSibling(node);
            if (previousNode == null) return this;
            return ReplaceNode(folder, folder.SwapChildNodes(node, previousNode));
        }

        public ShoppingList RemoveComponent(IComponent component)
        {
            var newComponents = Components.ToList();
            newComponents.Remove(component);
            return WithComponents(newComponents);
        }

        public ShoppingList ReplaceNode(IShoppingNode oldNode, IShoppingNode newNode)
        {
            if (oldNode is IShoppingFolder oldFolder && oldFolder.Id == Content.Id && newNode is IShoppingFolder newFolder)
                return WithContent(newFolder);

            return WithContent(Content.ReplaceNode(oldNode, newNode));
        }

        public ShoppingList WithTitle(string title) => 
            WithContent(Content.WithTitle(title));
        
        public ShoppingList WithContent(IShoppingFolder content) => 
            content == Content ? this : new ShoppingList(Id, Name, content, Components);
        
        public ShoppingList WithComponents(IReadOnlyList<IComponent> components) => 
            components == Components ? this : new ShoppingList(Id, Name, Content, components);
        
        public override IDocument WithContent(IContainerNode content) => WithContent(content as IShoppingFolder);

        IShoppingList IShoppingList.AddComponent(IComponent component) => AddComponent(component);
        IShoppingList IShoppingList.RemoveComponent(IComponent component) => RemoveComponent(component);
        IShoppingList IShoppingList.WithContent(IShoppingFolder content) => WithContent(content);
        IShoppingList IShoppingList.WithComponents(IReadOnlyList<IComponent> components) => WithComponents(components);
    }
}
