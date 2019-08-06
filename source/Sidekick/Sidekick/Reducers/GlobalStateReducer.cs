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
                case AddFolderAction _:
                    return new GlobalState {
                        NextFolderNumber = state.NextFolderNumber + 1,
                        NextLineNumber = state.NextLineNumber
                    };
                case AddLineAction _:
                    return new GlobalState {
                        NextFolderNumber = state.NextFolderNumber,
                        NextLineNumber = state.NextLineNumber + 1
                    };
                default:
                    return state;
            }
        }
    }
}
