using Xodium.Redux;

namespace Sidekick.Actions
{
    public class SelectNodeAction : ReduxAction<SelectNodeAction.Properties>
    {
        public SelectNodeAction(string nodeId)
            : base(typeof(SelectNodeAction).FullName, new Properties(nodeId))
        {
        }

        public struct Properties
        {
            public Properties(string nodeId)
            {
                NodeId = nodeId;
            }

            public string NodeId;
        }
    }
}
