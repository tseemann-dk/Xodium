using System;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xodium.Services;

namespace Xodium.Platform.Xamarin.Services
{
    public class SynchronizerService : ISynchronizerService
    {
        public SynchronizerService()
        {
            MainContext = SynchronizationContext.Current;
        }

        public SynchronizationContext MainContext { get; }

        public Task BeginInvokeOnMainThread(Action action)
        {
            Device.BeginInvokeOnMainThread(action);
            return Task.CompletedTask;
        }
    }
}
