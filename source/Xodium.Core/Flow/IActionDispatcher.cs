using System;
using System.Threading.Tasks;

namespace Xodium.Flow
{
    public delegate object Dispatcher(IAction action);
    public delegate Task ActionsCreator<TState>(Dispatcher dispatcher, Func<TState> getState);

    public interface IActionDispatcher
    {
        void Dispatch(IAction action);
    }

    public interface IActionDispatcher<TState> : IActionDispatcher
    {
        Task DispatchAsync(ActionsCreator<TState> actionsCreator);
    }
}
