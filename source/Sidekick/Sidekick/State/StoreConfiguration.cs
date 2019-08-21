using Redux;
using Sidekick.Features.Shopper.Middleware;
using Sidekick.Features.Shopper.Reducers;

namespace Sidekick.State
{
    static class StoreConfiguration
    {
        public static readonly Reducer<AppState>[] Reducers =
        {
            GlobalStateReducer.Execute,
            ShoppingSessionReducer.Execute,
            ShoppingListReducer.Execute,
            ComponentLookupReducer.Execute
        };

        public static readonly Middleware<AppState>[] Middlewares =
        {
            ComponentLookupMiddleware.CreateMiddleware(),
            ShoppingListMiddleware.CreateMiddleware()
        };
    }
}
