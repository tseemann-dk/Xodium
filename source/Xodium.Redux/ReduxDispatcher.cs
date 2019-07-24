using System;
using Xodium.Flow;

namespace Xodium.Redux
{
    public class ReduxDispatcher<TState> : IActionDispatcher
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
    }
}
