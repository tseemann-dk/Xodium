using Sidekick.Models;
using Sidekick.Extensions;
using System.Linq;
using Xodium.Productivity.Content.Models;
using System;

namespace Sidekick.Transformers
{
    public static class FolderTransformer
    {
        public static (IFolder, string) AddLine(
            IFolder folder,
            DateTime date,
            string text,
            double quantity,
            double value,
            string afterNodeId)
        {
            IFolder newFolder;
            var line = new Line(date, text, quantity, value);

            if (afterNodeId == null)
            {
                newFolder = folder.InsertNode(0, line);
            }
            else
            {
                var afterNode = folder.Nodes.FirstOrDefault(x => x.Id == afterNodeId);
                var index = folder.GetIndexOfNode(afterNode);

                newFolder = index >= folder.Nodes.Count
                    ? folder.AddNode(line)
                    : folder.InsertNode(index + 1, line);
            }

            return (newFolder, line.Id);
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
