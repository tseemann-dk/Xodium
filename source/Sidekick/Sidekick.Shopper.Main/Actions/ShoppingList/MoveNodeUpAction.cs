using System;
using Xodium.Redux;

namespace Sidekick.Shopper.Actions.ShoppingList
{
    public class MoveNodeUpAction : ReduxAction<MoveNodeUpAction.Properties>
    {
        public MoveNodeUpAction(string folderId, string nodeId)
            : base(typeof(MoveNodeUpAction).FullName, new Properties(folderId, nodeId))
        {
        }

        public struct Properties
        {
            public Properties(string folderId, string nodeId)
            {
                FolderId = folderId ?? throw new ArgumentNullException(nameof(folderId));
                NodeId = nodeId ?? throw new ArgumentNullException(nameof(nodeId));
            }

            public string FolderId;
            public string NodeId;
        }
    }
}
