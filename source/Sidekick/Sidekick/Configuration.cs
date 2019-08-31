using Sidekick.Shopper.Middleware;
using Sidekick.State;

namespace Sidekick
{
    static class Configuration
    {
        public static readonly Redux.Middleware<AppState>[] Middlewares =
        {
            ComponentLookupMiddleware.CreateMiddleware(),
            ShoppingListMiddleware.CreateMiddleware()
        };
    }
}
