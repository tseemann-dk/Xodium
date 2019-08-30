using System;
using System.Threading.Tasks;

namespace Xodium.Flow
{
    public delegate object Dispatcher(object action);
    public delegate TState Reducer<TState>(TState state, object action);
    public delegate Task ActionsCreator<TState>(Dispatcher dispatcher, Func<TState> getState);
}
