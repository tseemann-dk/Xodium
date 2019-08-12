using Sidekick.Actions;
using Sidekick.Models;
using Sidekick.Transformers;
using Xodium.Productivity.Content.Models;

namespace Sidekick.Reducers
{
    public class ArchiveStateReducer
    {
        public static ArchiveState Execute(ArchiveState state, object action)
        {
            IArchive archive = state.Document;
            IFolder newFolder = null;
            IElement newElement = null;

            var currentFolderId = state.CurrentFolderId;
            var focusedNodeId = state.FocusedNodeId;
            var currentFolder = state.Document.Content.FindNode<IFolder>(x => x.Id == state.CurrentFolderId);

            switch (action)
            {
                case EnterFolderAction a:
                    currentFolderId = a.Payload.FolderId;
                    focusedNodeId = null;
                    break;

                case ExitFolderAction a:
                    var parentId = currentFolder?.GetParent(state.Document.Content)?.Id;
                    if (parentId != null)
                    {
                        focusedNodeId = currentFolderId;
                        currentFolderId = parentId;
                    }
                    break;

                case FocusNodeAction a:
                    focusedNodeId = a.Payload.NodeId;
                    break;

                case ChangeFolderTitleAction a:
                    newFolder = FolderTransformer.ChangeTitle(
                        currentFolder, 
                        a.Payload.NewTitle);
                    break;

                case AddElementAction a:
                    newElement = a.Payload.Element;
                    break;

                case AddFolderAction a:
                    (newFolder, focusedNodeId) = FolderTransformer.AddFolder(
                        currentFolder, 
                        a.Payload.Number, 
                        a.Payload.Text, 
                        a.Payload.Quantity, 
                        a.Payload.InsertAfterNodeId);
                    break;

                case AddShortcutAction a:
                    (newFolder, focusedNodeId) = FolderTransformer.AddShortcut(
                        currentFolder, 
                        a.Payload.Target, 
                        a.Payload.Quantity, 
                        a.Payload.Text, 
                        a.Payload.Value, 
                        a.Payload.InsertAfterNodeId);
                    break;

                case DeleteNodeAction a:
                    (newFolder, focusedNodeId) = FolderTransformer.DeleteNode(
                        currentFolder, 
                        a.Payload.NodeId);
                    break;

                case MoveNodeDownAction a:
                    (newFolder, focusedNodeId) = FolderTransformer.MoveNodeDown(
                        currentFolder, 
                        a.Payload.NodeId);
                    break;

                case MoveNodeUpAction a:
                    (newFolder, focusedNodeId) = FolderTransformer.MoveNodeUp(
                        currentFolder, 
                        a.Payload.NodeId);
                    break;
            }

            if (newFolder != null)
            {
                archive = currentFolder.Id == archive.Content.Id
                    ? state.Document.WithContent(newFolder) as Archive
                    : state.Document.ReplaceNode(currentFolder, newFolder) as Archive;
            }

            if (newElement != null)
            {
                archive = archive.AddElement(newElement);
            }

            return new ArchiveState
            {
                Document = archive as Archive,
                CurrentFolderId = currentFolderId,
                FocusedNodeId = focusedNodeId
            };
        }
    }
}
