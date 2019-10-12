using Xodium.Redux;

namespace Sidekick.Actions.Global
{
    public class GetNextFolderNumberAction : ReduxAction
    {
        public GetNextFolderNumberAction() 
            : base(typeof(GetNextFolderNumberAction).FullName)
        {
        }
    }
}
