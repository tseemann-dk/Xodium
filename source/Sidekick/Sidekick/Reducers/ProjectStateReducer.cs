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
            var project = state.Project;
            var currentFolderId = state.CurrentFolderId;
            var selectedNodeId = state.SelectedNodeId;
            var currentFolder = state.Project.Content.FindNode<IFolder>(x => x.Id == state.CurrentFolderId);
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
                        addLineAction.Payload.Element, 
                        addLineAction.Payload.Quantity, 
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
                    ? state.Project.Clone(newFolder) as Project
                    : state.Project.ReplaceNode(currentFolder, newFolder) as Project;
            }

            return new ProjectState
            {
                Project = project,
                CurrentFolderId = currentFolderId,
                SelectedNodeId = selectedNodeId
            };
        }
    }
}
