using System;
using System.Threading.Tasks;
using Redux;

namespace Xodium.Redux
{
    public delegate Task AsyncActionsCreator<TState>(Dispatcher dispatcher, Func<TState> getState);

    public static class StoreExtensions
    {
        public static Task DispatchAsync<TState>(this IStore<TState> store, AsyncActionsCreator<TState> actionsCreator)
        {
            return actionsCreator(store.Dispatch, store.GetState);
        }
    }
}
