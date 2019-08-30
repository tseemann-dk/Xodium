using System;
using System.Threading.Tasks;
using Redux.Reactive;
using Xodium.Flow;

namespace Xodium.Redux
{
    public class ReduxStore<TState> : IStore<TState>
    {
        public ReduxStore(global::Redux.IStore<TState> store)
        {
            Store = store ?? throw new ArgumentNullException(nameof(store));
        }

        public ReduxStore(
            Func<global::Redux.Reducer<TState>, global::Redux.IStore<TState>> storeFactory,
            Reducer<TState> reducer)
            : this(storeFactory(ConvertReducer(reducer)))
        {
        }

        private static global::Redux.Reducer<TState> ConvertReducer(Reducer<TState> reducer)
        {
            return (state, action) => reducer(state, action);
        }

        public global::Redux.IStore<TState> Store { get; }
        public IObservable<TState> StateChanges => Store.ObserveState();

        public object Dispatch(object action) => Store.Dispatch(action);
        
        public Task DispatchAsync(ActionsCreator<TState> actionsCreator) => 
            Store.DispatchAsync((dispatcher, getState) => actionsCreator(action => dispatcher(action), getState));
        
        public TState GetState() => Store.GetState();

        object IStore.GetState() => GetState();
    }
}
