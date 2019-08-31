using Sidekick.Shopper.Actions.ShoppingList;

namespace Sidekick.State
{
    public class GlobalStateReducer
    {
        public static GlobalState Execute(GlobalState state, object action)
        {
            // TODO: Handle IncreaseGroupNumber/IncreaseComponentNumber instead

            switch (action)
            {
                case AddGroupAction _:
                    return state.WithNextGroupNumber();
                case AddItemAction _:
                    return state.WithNextComponentNumber();
            }

            return state;
        }
    }
}
