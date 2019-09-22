using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Xodium.Mvvm;

namespace Xodium.Platform.Uno.Services
{
    public class DialogService : IDialogService
    {
        private readonly Lazy<IViewRegistry> viewRegistry;

        public DialogService(Func<IViewRegistry> viewRegistryProvider)
        {
            if (viewRegistryProvider is null)
                throw new ArgumentNullException(nameof(viewRegistryProvider));

            viewRegistry = new Lazy<IViewRegistry>(() => viewRegistryProvider());
        }

        protected IViewRegistry ViewRegistry => viewRegistry.Value ?? throw new NotSupportedException("ViewRegistry not assigned");

        public Task<bool> DisplayAlert(string title, string message, string accept = null, string cancel = null)
        {
            throw new NotImplementedException();
        }

        public Task<UserAction> DisplayDialog(object viewModel, UserAction primaryAction = null, UserAction secondaryAction = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task DisplayException(string title, Exception exception)
        {
            throw new NotImplementedException();
        }

        public Task<string> DisplayPrompt(string title, string message, string value, string accept = null, string cancel = null)
        {
            throw new NotImplementedException();
        }

        public Task<UserAction> SelectAction(string title, string cancel, IEnumerable<UserAction> actions)
        {
            throw new NotImplementedException();
        }

        private async Task<bool> DisplayDialog(object content)
        {
            var dialog = new ContentDialog
            {
                Content = content
            };

            return await dialog.ShowAsync() == ContentDialogResult.Primary;
        }
    }
}
