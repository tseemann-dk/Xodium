using Xodium.Redux;

namespace Sidekick.Shopper.Actions.ShoppingList
{
    public class ChangeFolderTitleAction : ReduxAction<ChangeFolderTitleAction.Properties>
    {
        public ChangeFolderTitleAction(string folderId, string newTitle)
            : base(typeof(ChangeFolderTitleAction).FullName, new Properties(folderId, newTitle))
        {
        }

        public struct Properties
        {
            public Properties(string folderId, string newTitle)
            {
                FolderId = folderId ?? throw new System.ArgumentNullException(nameof(folderId));
                NewTitle = newTitle ?? throw new System.ArgumentNullException(nameof(newTitle));
            }

            public string FolderId;
            public string NewTitle;
        }
    }
}
