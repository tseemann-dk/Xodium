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
                        NextElementNumber = state.NextElementNumber
                    };
                case AddShortcutAction _:
                    return new GlobalState {
                        NextFolderNumber = state.NextFolderNumber,
                        NextElementNumber = state.NextElementNumber + 1
                    };
                default:
                    return state;
            }
        }
    }
}
