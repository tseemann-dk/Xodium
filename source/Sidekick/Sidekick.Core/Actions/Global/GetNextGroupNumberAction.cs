using Xodium.Redux;

namespace Sidekick.Actions.Global
{
    public class GetNextGroupNumberAction : ReduxAction
    {
        public GetNextGroupNumberAction() 
            : base(typeof(GetNextGroupNumberAction).FullName)
        {
        }
    }
}
