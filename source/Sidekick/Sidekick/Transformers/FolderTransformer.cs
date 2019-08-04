using Sidekick.Models;
using Sidekick.Extensions;
using System.Linq;
using Xodium.Productivity.Content.Models;
using System;

namespace Sidekick.Transformers
{
    public static class FolderTransformer
    {
        public static (IFolder, string) AddFolder(
            IFolder parentFolder,
            string number,
            string text,
            double quantity,
            string afterNodeId)
        {
            var folder = new Folder(number, text, quantity);
            var newParentFolder = AddNode(parentFolder, folder, afterNodeId);
            return (newParentFolder, folder.Id);
        }

        public static (IFolder, string) AddLine(
            IFolder parentFolder,
            DateTime date,
            string text,
            double quantity,
            double value,
            string afterNodeId)
        {
            var line = new Line(date, text, quantity, value);
            var newFolder = AddNode(parentFolder, line, afterNodeId);
            return (newFolder, line.Id);
        }

        public static IFolder AddNode(
            IFolder parentFolder,
            INode node,
            string afterNodeId)
        {
            IFolder newParentFolder;

            if (afterNodeId == null)
            {
                newParentFolder = parentFolder.InsertNode(0, node);
            }
            else
            {
                var afterNode = parentFolder.Nodes.FirstOrDefault(x => x.Id == afterNodeId);
                var index = parentFolder.GetIndexOfNode(afterNode);

                newParentFolder = index >= parentFolder.Nodes.Count
                    ? parentFolder.AddNode(node)
                    : parentFolder.InsertNode(index + 1, node);
            }

            return newParentFolder;
        }

        public static (IFolder, string) DeleteNode(IFolder folder, string nodeId)
        {
            var node = folder.Nodes.First(x => x.Id == nodeId);
            var neighborId = (folder.GetNextNode(node) ?? folder.GetPreviousNode(node))?.Id;
            return (folder.RemoveNode(node), neighborId);
        }

        public static IFolder ChangeTitle(IFolder folder, string newTitle)
        {
            return folder.WithText(newTitle);
        }
    }
}
