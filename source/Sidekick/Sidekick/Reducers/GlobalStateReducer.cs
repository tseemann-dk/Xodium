using Sidekick.Actions;
using Sidekick.Models;

namespace Sidekick.Reducers
{
    public class GlobalStateReducer
    {
        public static GlobalState Execute(GlobalState state, object action)
        {
            switch (action)
            {
                case AddLineAction _:
                    return new GlobalState {
                        NextExpenseNumber = state.NextExpenseNumber + 1
                    };
                default:
                    return state;
            }
        }
    }
}
