using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xodium.Mvvm;

namespace Xodium.Platform.Xamarin.Services
{
    public class DialogService : IDialogService
    {
        private readonly Page rootPage;
        private readonly Func<IViewRegistry> getViewRegistry;

        public DialogService(Page rootPage, Func<IViewRegistry> getViewRegistry)
        {
            this.rootPage = rootPage ?? throw new ArgumentNullException(nameof(rootPage));
            this.getViewRegistry = getViewRegistry ?? throw new ArgumentNullException(nameof(getViewRegistry));
        }

        public async Task<bool> DisplayAlert(string title, string message, string accept, string cancel)
        {
            if (cancel != null) 
                return await rootPage.DisplayAlert(title, message, accept, cancel);
            
            await rootPage.DisplayAlert(title, message, accept ?? "OK");
            return false;
        }

        public Task DisplayException(string title, Exception exception)
        {
            var message = Debugger.IsAttached ? exception.ToString() : exception.Message;
            return DisplayAlert(title, message, null, null);
        }

        public Task<string> DisplayPrompt(string title, string message, string value, string accept, string cancel)
        {
            throw new NotImplementedException();
        }

        public async Task<UserAction> DisplayDialog(object viewModel, UserAction primaryAction, UserAction secondaryAction, CancellationToken cancellationToken)
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));

            var navigation = rootPage.Navigation;

            if (!(getViewRegistry()?.GetViewFor(viewModel) is View view))
                throw new ArgumentException("No supported view was found", nameof(viewModel));

            view.SetValue(Grid.RowProperty, 0);
            view.SetValue(Grid.ColumnSpanProperty, 2);

            async void okAction()
            {
                primaryAction.Execute();
                await navigation.PopModalAsync();
            }

            async void cancelAction()
            {
                await navigation.PopModalAsync();
            }

            var okButton = new Button { Text = "OK" };
            var cancelButton = new Button { Text = "Cancel" };

            okButton.Clicked += (s, e) => okAction();
            cancelButton.Clicked += (s, e) => cancelAction();

            okButton.SetValue(Grid.RowProperty, 1);
            okButton.SetValue(Grid.ColumnProperty, 0);

            cancelButton.SetValue(Grid.RowProperty, 1);
            cancelButton.SetValue(Grid.ColumnProperty, 1);

            var page = new ContentPage
            {
                Content = new Grid
                {
                    Margin = new Thickness(12),
                    RowDefinitions =
                    {
                        new RowDefinition { Height = GridLength.Star },
                        new RowDefinition { Height = GridLength.Auto }
                    },
                    ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = GridLength.Star },
                        new ColumnDefinition { Width = GridLength.Star }
                    },
                    Children = { view, okButton, cancelButton }
                }
            };

            if (viewModel is INavigationDestination target)
            {
                await target.NavigateTo();
            }

            await rootPage.Navigation.PushModalAsync(page);
            return null;
        }

        public async Task<UserAction> SelectAction(string title, string cancel, IEnumerable<UserAction> operations)
        {
            var activeOperations = operations.Where(o => o.Command == null || o.Command.CanExecute(o.Argument)).ToList();
            var selectedOperationName = await rootPage.DisplayActionSheet(title, cancel, null, activeOperations.Select(o => o.Name).ToArray());
            var operation = activeOperations.FirstOrDefault(a => a.Name == selectedOperationName);
            
            if (operation == null) 
                return null;

            if (Device.RuntimePlatform == Device.iOS)
            {
                // Give action sheet dialog some time to complete on iOS
                await Task.Delay(500);
            }

            operation.Command?.Execute(operation.Argument);
            return operation;
        }
    }
}
