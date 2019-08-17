using System;
using Xodium.Redux;

namespace Sidekick.Features.Shopper.Actions
{
    public class MoveNodeDownAction : ReduxAction<MoveNodeDownAction.Properties>
    {
        public MoveNodeDownAction(string parentGroupId, string nodeId)
            : base(typeof(MoveNodeDownAction).FullName, new Properties(parentGroupId, nodeId))
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
