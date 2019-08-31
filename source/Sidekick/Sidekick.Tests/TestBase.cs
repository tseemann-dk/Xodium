using Sidekick.Reducers;
using Sidekick.Shopper.Middleware;
using Sidekick.State;
using Xodium.Flow;
using Xodium.Redux;

namespace Sidekick.Tests
{
    public abstract class TestBase
    {
        public TestBase()
        {
            var middlewares = new[]
            {
                ComponentLookupMiddleware.CreateMiddleware(),
                ShoppingListMiddleware.CreateMiddleware()
            };

            Store = new ReduxStore<AppState>(
                r => new Redux.Store<AppState>(r, AppStateGenerator.GenerateDefaultState(), middlewares), 
                AppStateReducer.Reduce
            );
        }
        
        protected IStore<AppState> Store { get; }
    }
}
