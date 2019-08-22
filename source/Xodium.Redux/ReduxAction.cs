using Xodium.Flow;

namespace Xodium.Redux
{
    public class ReduxAction<T> : ActionBase<T>
    {
        public ReduxAction(string actionType, T payload) 
            : base(actionType, payload)
        {
        }
    }

    public class ReduxAction : ActionBase
    {
        public ReduxAction(string actionType) 
            : base(actionType)
        {
        }
    }
}
