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
                case SelectFolderAction selectFolderAction:
                    currentFolderId = selectFolderAction.Payload.FolderId;
                    break;

                case SelectNodeAction selectNodeAction:
                    selectedNodeId = selectNodeAction.Payload.NodeId;
                    break;

                case ChangeFolderTitleAction changeFolderTitleAction:
                    newFolder = FolderTransformer.ChangeTitle(
                        currentFolder, 
                        changeFolderTitleAction.Payload.NewTitle);
                    break;

                case AddLineAction addLineAction:
                    (newFolder, selectedNodeId) = FolderTransformer.AddLine(
                        currentFolder, 
                        addLineAction.Payload.Date, 
                        addLineAction.Payload.Text, 
                        addLineAction.Payload.Quantity, 
                        addLineAction.Payload.Value, 
                        addLineAction.Payload.InsertAfterNodeId);
                    break;

                case DeleteNodeAction deleteNodeAction:
                    (newFolder, selectedNodeId) = FolderTransformer.DeleteNode(
                        currentFolder, 
                        deleteNodeAction.Payload.NodeId);
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
