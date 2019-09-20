using Sidekick.Actions.Global;

namespace Sidekick.State
{
    public class GlobalStateReducer
    {
        public static GlobalState Reduce(GlobalState state, object action)
        {
            switch (action)
            {
                case GetNextComponentNumberAction _:
                    return state.WithNextComponentNumber();
                case GetNextGroupNumberAction _:
                    return state.WithNextGroupNumber();
                default:
                    return state;
            }
        }
    }
}
