using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Xodium.Extensions;
using Xodium.Mvvm;

// TODO: Finish implementation

namespace Xodium.Platform.Windows.Wpf.Services
{
    public class DialogService : IDialogService
    {
        private readonly Func<IViewRegistry> getViewRegistry;

        public DialogService(Func<IViewRegistry> getViewRegistry)
        {
            this.getViewRegistry = getViewRegistry ?? throw new ArgumentNullException(nameof(getViewRegistry));
        }

        public Task<bool> DisplayAlert(string title, string message, string accept = null, string cancel = null)
        {
            return Task.FromResult(MessageBox.Show(message, title, MessageBoxButton.OKCancel) == MessageBoxResult.OK);
        }

        public Task<UserAction> DisplayDialog(string title, object viewModel, UserAction primaryAction = null, UserAction secondaryAction = null, CancellationToken cancellationToken = default)
        {
            // TODO: Finish

            var content = getViewRegistry().GetViewFor(viewModel);

            var window = new Window
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                WindowStyle = WindowStyle.SingleBorderWindow,
                WindowState = WindowState.Normal,
                SizeToContent = SizeToContent.WidthAndHeight,
                Content = content
            };

            window.ShowDialog();

            return Task.FromResult(new UserAction("", () => { }));
        }

        public Task DisplayException(string title, Exception exception)
        {
            MessageBox.Show(exception.GetMessageList(), title, MessageBoxButton.OK, MessageBoxImage.Error);
            return Task.CompletedTask;
        }

        public Task<string> DisplayPrompt(string title, string message, string value, string accept = null, string cancel = null)
        {
            var answer = MessageBox.Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes);
            var result = answer == MessageBoxResult.Yes ? (accept ?? "OK") : (cancel ?? "Cancel");
            return Task.FromResult(result);
        }

        public Task<UserAction> SelectAction(string title, string cancel, IEnumerable<UserAction> actions)
        {
            throw new NotImplementedException();
        }
    }
}
