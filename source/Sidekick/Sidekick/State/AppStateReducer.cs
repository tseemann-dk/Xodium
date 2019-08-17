using Sidekick.Features.Shopper.Reducers;

namespace Sidekick.State
{
    public class AppStateReducer
    {
        public static AppState Execute(AppState state, object action)
        {
            return new AppState
            {
                Global = GlobalStateReducer.Execute(state.Global, action),
                CurrentShoppingSession = ShoppingSessionReducer.Execute(state.CurrentShoppingSession, action)
            };
        }
    }
}
