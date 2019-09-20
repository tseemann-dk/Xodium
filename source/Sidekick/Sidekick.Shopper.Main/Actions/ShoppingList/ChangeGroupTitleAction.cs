using Xodium.Redux;

namespace Sidekick.Shopper.Actions.ShoppingList
{
    public class ChangeGroupTitleAction : ReduxAction<ChangeGroupTitleAction.Properties>
    {
        public ChangeGroupTitleAction(string groupId, string newTitle)
            : base(typeof(ChangeGroupTitleAction).FullName, new Properties(groupId, newTitle))
        {
        }

        public struct Properties
        {
            public Properties(string groupId, string newTitle)
            {
                GroupId = groupId ?? throw new System.ArgumentNullException(nameof(groupId));
                NewTitle = newTitle ?? throw new System.ArgumentNullException(nameof(newTitle));
            }

            public string GroupId;
            public string NewTitle;
        }
    }
}
