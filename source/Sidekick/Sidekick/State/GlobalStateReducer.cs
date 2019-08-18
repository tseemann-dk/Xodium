using Sidekick.Features.Shopper.Actions;

namespace Sidekick.State
{
    public class GlobalStateReducer
    {
        public static AppState Execute(AppState state, object action)
        {
            // TODO: Handle IncreaseGroupNumber/IncreaseComponentNumber instead

            AppState newGlobalState(GlobalState global) => state.WithGlobal(global);

            switch (action)
            {
                case AddGroupAction _:
                    return newGlobalState(state.Global.WithNextGroupNumber());
                case AddItemAction _:
                    return newGlobalState(state.Global.WithNextComponentNumber());
            }

            return state;
        }
    }
}
