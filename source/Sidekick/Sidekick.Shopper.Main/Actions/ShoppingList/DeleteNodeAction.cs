using System;
using Xodium.Redux;

namespace Sidekick.Shopper.Actions.ShoppingList
{
    public class DeleteNodeAction : ReduxAction<DeleteNodeAction.Properties>
    {
        public DeleteNodeAction(string folderId, string nodeId)
            : base(typeof(DeleteNodeAction).FullName, new Properties(folderId, nodeId))
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
