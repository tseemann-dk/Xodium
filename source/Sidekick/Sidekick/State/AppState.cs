using Sidekick.Features.Shopper.Models;

namespace Sidekick.State

{
    public struct AppState
    {
        public AppState(GlobalState global, ShoppingSession shoppingSession)
        {
            Global = global;
            ShoppingSession = shoppingSession;
        }

        public GlobalState Global;
        public ShoppingSession ShoppingSession;

        public AppState WithGlobal(GlobalState global) => new AppState(global, ShoppingSession);
        public AppState WithShoppingSession(ShoppingSession shoppingSession) => new AppState(Global, shoppingSession);
    }
}
