using Sidekick.Features.Shopper.Actions;

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
                    return new GlobalState {
                        NextGroupNumber = state.NextGroupNumber + 1,
                        NextComponentNumber = state.NextComponentNumber
                    };
                case AddShoppingItemAction _:
                    return new GlobalState {
                        NextGroupNumber = state.NextGroupNumber,
                        NextComponentNumber = state.NextComponentNumber + 1
                    };
                default:
                    return state;
            }
        }
    }
}
