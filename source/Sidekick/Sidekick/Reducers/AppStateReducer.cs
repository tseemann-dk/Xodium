using Sidekick.Shopper.Reducers;
using Sidekick.State;

namespace Sidekick.Reducers
{
    public static class AppStateReducer
    {
        public static AppState Reduce(AppState state, object action)
        {
            return state
                .WithGlobal(GlobalStateReducer.Reduce(state.Global, action))
                .WithShoppingSession(ShoppingSessionReducer.Reduce(state.ShoppingSession, action));
        }
    }
}
