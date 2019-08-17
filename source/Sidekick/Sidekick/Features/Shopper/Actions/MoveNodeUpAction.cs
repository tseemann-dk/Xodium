using System;
using Xodium.Redux;

namespace Sidekick.Features.Shopper.Actions
{
    public class MoveNodeUpAction : ReduxAction<MoveNodeUpAction.Properties>
    {
        public MoveNodeUpAction(string parentGroupId, string nodeId)
            : base(typeof(MoveNodeUpAction).FullName, new Properties(parentGroupId, nodeId))
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
