using System;
using System.Threading.Tasks;

namespace Xodium.Flow
{
    public interface IStore
    {
        object Dispatch(object action);
        object GetState();
    }

    public interface IStore<TState> : IStore
    {
        IObservable<TState> StateChanges { get; }
        Task DispatchAsync(ActionsCreator<TState> actionsCreator);
        new TState GetState();
    }
}
