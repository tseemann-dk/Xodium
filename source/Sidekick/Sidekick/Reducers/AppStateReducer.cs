using Sidekick.Shopper.Reducers;
using Sidekick.State;

namespace Sidekick.Reducers
{
    public static class AppStateReducer
    {
        public static AppState Execute(AppState state, object action)
        {
            return state
                .WithGlobal(GlobalStateReducer.Execute(state.Global, action))
                .WithShoppingSession(ShoppingSessionReducer.Execute(state.ShoppingSession, action));
        }
    }
}
