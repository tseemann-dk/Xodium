using Sidekick.Actions;
using Sidekick.Models;
using Sidekick.Transformers;
using Xodium.Productivity.Content.Models;

namespace Sidekick.Reducers
{
    public class ProjectStateReducer
    {
        public static ProjectState Execute(ProjectState state, object action)
        {
            var project = state.Document;
            var currentFolderId = state.CurrentFolderId;
            var selectedNodeId = state.SelectedNodeId;
            var currentFolder = state.Document.Content.FindNode<IFolder>(x => x.Id == state.CurrentFolderId);
            IFolder newFolder = null;

            switch (action)
            {
                case EnterFolderAction a:
                    currentFolderId = a.Payload.FolderId;
                    selectedNodeId = null;
                    break;

                case ExitFolderAction a:
                    var parentId = currentFolder?.GetParent(state.Document.Content)?.Id;
                    if (parentId != null)
                    {
                        selectedNodeId = currentFolderId;
                        currentFolderId = parentId;
                    }
                    break;

                case SelectNodeAction a:
                    selectedNodeId = a.Payload.NodeId;
                    break;

                case ChangeFolderTitleAction a:
                    newFolder = FolderTransformer.ChangeTitle(
                        currentFolder, 
                        a.Payload.NewTitle);
                    break;

                case AddFolderAction a:
                    (newFolder, selectedNodeId) = FolderTransformer.AddFolder(
                        currentFolder, 
                        a.Payload.Number, 
                        a.Payload.Text, 
                        a.Payload.Quantity, 
                        a.Payload.InsertAfterNodeId);
                    break;

                case AddLineAction a:
                    (newFolder, selectedNodeId) = FolderTransformer.AddLine(
                        currentFolder, 
                        a.Payload.Date, 
                        a.Payload.Text, 
                        a.Payload.Quantity, 
                        a.Payload.Value, 
                        a.Payload.InsertAfterNodeId);
                    break;

                case DeleteNodeAction a:
                    (newFolder, selectedNodeId) = FolderTransformer.DeleteNode(
                        currentFolder, 
                        a.Payload.NodeId);
                    break;
            }

            if (newFolder != null)
            {
                project = currentFolder.Id == project.Content.Id
                    ? state.Document.Clone(newFolder) as Project
                    : state.Document.ReplaceNode(currentFolder, newFolder) as Project;
            }

            return new ProjectState
            {
                Document = project,
                CurrentFolderId = currentFolderId,
                SelectedNodeId = selectedNodeId
            };
        }
    }
}
