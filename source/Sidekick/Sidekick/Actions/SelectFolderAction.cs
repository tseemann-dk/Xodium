using Xodium.Redux;

namespace Sidekick.Actions
{
    public class SelectFolderAction : ReduxAction<SelectFolderAction.Properties>
    {
        public SelectFolderAction(string folderId)
            : base(typeof(SelectFolderAction).FullName, new Properties(folderId))
        {
        }

        public struct Properties
        {
            public Properties(string folderId)
            {
                FolderId = folderId;
            }

            public string FolderId;
        }
    }
}
