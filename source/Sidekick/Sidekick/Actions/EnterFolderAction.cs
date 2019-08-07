using Xodium.Redux;

namespace Sidekick.Actions
{
    public class EnterFolderAction : ReduxAction<EnterFolderAction.Properties>
    {
        public EnterFolderAction(string folderId)
            : base(typeof(EnterFolderAction).FullName, new Properties(folderId))
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
