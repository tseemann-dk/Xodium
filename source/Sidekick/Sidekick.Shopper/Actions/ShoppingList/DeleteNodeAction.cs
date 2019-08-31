using System;
using Xodium.Redux;

namespace Sidekick.Shopper.Actions.ShoppingList
{
    public class DeleteNodeAction : ReduxAction<DeleteNodeAction.Properties>
    {
        public DeleteNodeAction(string parentGroupId, string nodeId)
            : base(typeof(DeleteNodeAction).FullName, new Properties(parentGroupId, nodeId))
        {
        }

        public struct Properties
        {
            public Properties(string parentGroupId, string nodeId)
            {
                ParentGroupId = parentGroupId ?? throw new ArgumentNullException(nameof(parentGroupId));
                NodeId = nodeId ?? throw new ArgumentNullException(nameof(nodeId));
            }

            public string ParentGroupId;
            public string NodeId;
        }
    }
}
