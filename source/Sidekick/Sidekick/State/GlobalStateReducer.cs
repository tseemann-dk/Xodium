using Sidekick.Features.Shopper.Actions.ShoppingList;

namespace Sidekick.State
{
    public class GlobalStateReducer
    {
        public static AppState Execute(AppState state, object action)
        {
            // TODO: Handle IncreaseGroupNumber/IncreaseComponentNumber instead

            switch (action)
            {
                case AddGroupAction _:
                    return state.WithGlobal(state.Global.WithNextGroupNumber());
                case AddItemAction _:
                    return state.WithGlobal(state.Global.WithNextComponentNumber());
            }

            return state;
        }
    }
}
