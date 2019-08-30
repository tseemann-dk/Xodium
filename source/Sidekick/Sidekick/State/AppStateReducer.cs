using Sidekick.Features.Shopper.Reducers;

namespace Sidekick.State
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
