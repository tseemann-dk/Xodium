using Xodium.Redux;

namespace Sidekick.Actions
{
    public class FocusNodeAction : ReduxAction<FocusNodeAction.Properties>
    {
        public FocusNodeAction(string nodeId)
            : base(typeof(FocusNodeAction).FullName, new Properties(nodeId))
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
