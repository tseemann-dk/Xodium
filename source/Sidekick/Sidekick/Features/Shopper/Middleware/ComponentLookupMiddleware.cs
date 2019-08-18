using System;
using Redux;
using Sidekick.State;

namespace Sidekick.Features.Shopper.Middleware
{
    public static class ComponentLookupMiddleware
    {
        public static Func<Dispatcher, Dispatcher> Execute(IStore<AppState> store)
        {
            return dispatcher =>
            {
                // dispatcher(action)

                return dispatcher;
            };
        }
    }
}
