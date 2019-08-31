using Sidekick.Shopper.State;

namespace Sidekick.State

{
    public class AppState
    {
        public AppState(GlobalState global, ShoppingSession shoppingSession)
        {
            Global = global;
            ShoppingSession = shoppingSession;
        }

        public GlobalState Global { get; }
        public ShoppingSession ShoppingSession { get; }

        public AppState WithGlobal(GlobalState global) => 
            global == Global ? this : new AppState(global, ShoppingSession);

        public AppState WithShoppingSession(ShoppingSession shoppingSession) => 
            shoppingSession == ShoppingSession ? this : new AppState(Global, shoppingSession);
    }
}
