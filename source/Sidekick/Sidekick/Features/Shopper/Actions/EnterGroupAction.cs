using Xodium.Redux;

namespace Sidekick.Features.Shopper.Actions
{
    public class EnterGroupAction : ReduxAction<EnterGroupAction.Properties>
    {
        public EnterGroupAction(string groupId)
            : base(typeof(EnterGroupAction).FullName, new Properties(groupId))
        {
        }

        public struct Properties
        {
            public Properties(string groupId)
            {
                GroupId = groupId;
            }

            public string GroupId;
        }
    }
}
