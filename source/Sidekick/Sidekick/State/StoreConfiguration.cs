using Sidekick.Features.Shopper.Middleware;

namespace Sidekick.State
{
    static class StoreConfiguration
    {
        public static readonly Redux.Middleware<AppState>[] Middlewares =
        {
            ComponentLookupMiddleware.CreateMiddleware(),
            ShoppingListMiddleware.CreateMiddleware()
        };
    }
}
