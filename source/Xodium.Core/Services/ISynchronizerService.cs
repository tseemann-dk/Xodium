using System;
using System.Threading;
using System.Threading.Tasks;

namespace Xodium.Services
{
    public interface ISynchronizerService
    {
        SynchronizationContext MainContext { get; }
        Task BeginInvokeOnMainThread(Action action);
    }
}
