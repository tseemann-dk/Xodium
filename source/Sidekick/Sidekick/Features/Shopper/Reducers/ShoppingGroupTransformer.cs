using System.Linq;
using Sidekick.Features.Shopper.Models;
using Xodium.Productivity.Content.Models;

namespace Sidekick.Features.Shopper.Reducers
{
    public static class ShoppingGroupTransformer
    {
        public static (IShoppingGroup, string) AddGroup(
            IShoppingGroup parentGroup,
            string groupNumber,
            string title,
            double quantity,
            string afterNodeId)
        {
            var newGroup = new ShoppingGroup(groupNumber, title, quantity);
            var newParentGroup = AddNode(parentGroup, newGroup, afterNodeId);
            return (newParentGroup, newGroup.Id);
        }

        public static (IShoppingGroup, string) AddItem(
            IShoppingGroup parentGroup,
            IComponent component,
            double quantity,
            string text,
            double? price,
            string afterNodeId)
        {
            var newItem = new ShoppingItem(component, quantity, text, price);
            var newParentGroup = AddNode(parentGroup, newItem, afterNodeId);
            return (newParentGroup, newItem.Id);
        }

        private static IShoppingGroup AddNode(
            IShoppingGroup parentGroup,
            INode node,
            string afterNodeId)
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

            return newParentGroup;
        }

        public static (IShoppingGroup, string) DeleteNode(IShoppingGroup parentGroup, string nodeId)
        {
            var node = parentGroup.Nodes.First(x => x.Id == nodeId);
            var neighborId = (parentGroup.GetNextNode(node) ?? parentGroup.GetPreviousNode(node))?.Id;
            return (parentGroup.RemoveNode(node), neighborId);
        }

        public static (IShoppingGroup, string) MoveNodeDown(IShoppingGroup parentGroup, string nodeId)
        {
            var node = parentGroup.Nodes.First(x => x.Id == nodeId);
            var nextNode = parentGroup.GetNextNode(node);
            if (nextNode == null) return (parentGroup, nodeId);
            return (parentGroup.SwapChildNodes(node, nextNode), nodeId);
        }

        public static (IShoppingGroup, string) MoveNodeUp(IShoppingGroup parentGroup, string nodeId)
        {
            var node = parentGroup.Nodes.First(x => x.Id == nodeId);
            var previousNode = parentGroup.GetPreviousNode(node);
            if (previousNode == null) return (parentGroup, nodeId);
            return (parentGroup.SwapChildNodes(node, previousNode), nodeId);
        }

        public static IShoppingGroup ChangeTitle(IShoppingGroup group, string newTitle)
        {
            return group.WithTitle(newTitle);
        }
    }
}
