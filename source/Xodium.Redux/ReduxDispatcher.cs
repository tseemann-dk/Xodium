using System;
using System.Threading.Tasks;
using Xodium.Flow;

namespace Xodium.Redux
{
    public class ReduxDispatcher<TState> : IActionDispatcher<TState>
    {
        private readonly global::Redux.IStore<TState> store;

        public ReduxDispatcher(global::Redux.IStore<TState> store)
        {
            this.store = store ?? throw new ArgumentNullException(nameof(store));
        }

        public void Dispatch(IAction action)
        {
            store.Dispatch(action);
        }

        public Task DispatchAsync(ActionsCreator<TState> actionsCreator)
        {
            return store.DispatchAsync(async (dispatch, getState) =>
            {
                await actionsCreator(action => dispatch(action), getState);
            });
        }
    }
}
