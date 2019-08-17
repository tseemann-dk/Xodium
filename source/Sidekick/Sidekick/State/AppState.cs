using Sidekick.Features.Shopper.Models;

namespace Sidekick.State

{
    public struct AppState
    {
        public GlobalState Global;
        public ShoppingSession CurrentShoppingSession;
    }
}
