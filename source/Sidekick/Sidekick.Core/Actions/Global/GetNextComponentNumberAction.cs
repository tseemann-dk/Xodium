using Xodium.Redux;

namespace Sidekick.Actions.Global
{
    public class GetNextComponentNumberAction : ReduxAction
    {
        public GetNextComponentNumberAction() 
            : base(typeof(GetNextComponentNumberAction).FullName)
        {
        }
    }
}
