using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Xodium.Services;

namespace Xodium.Platform.Uwp.Services
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
            return CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => action?.Invoke()).AsTask();
        }
    }
}
