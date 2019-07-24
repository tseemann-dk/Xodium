using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Xodium.Services;

namespace Xodium.Platform.Windows.Services
{
    public class SynchronizerService : ISynchronizerService
    {
        public SynchronizerService(SynchronizationContext context = null)
        {
            MainContext = context ?? SynchronizationContext.Current;
        }

        public SynchronizationContext MainContext { get; }

        public async Task BeginInvokeOnMainThread(Action action)
        {
            await Application.Current.MainWindow.Dispatcher.BeginInvoke(action, null);
        }
    }
}
