using Xodium.Redux;

namespace Sidekick.Actions
{
    public class ExitFolderAction : ReduxAction<ExitFolderAction.Properties>
    {
        public ExitFolderAction()
            : base(typeof(ExitFolderAction).FullName, new Properties())
        {
        }

        public struct Properties
        {
        }
    }
}
