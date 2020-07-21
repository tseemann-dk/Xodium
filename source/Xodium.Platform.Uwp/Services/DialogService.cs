using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Xodium.Mvvm;
using Xodium.Platform.Uwp.Extensions;
using Xodium.Platform.Uwp.Triggers;

namespace Xodium.Platform.Uwp.Services
{
    public class DialogService : IDialogService
    {
        private readonly DependencyObject context;
        private readonly IViewRegistry viewRegistry;

        public DialogService(DependencyObject context, IViewRegistry viewRegistry)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.viewRegistry = viewRegistry ?? throw new ArgumentNullException(nameof(viewRegistry));
        }

        private static async Task<object> ShowDialog(MessageDialog dialog)
        {
            var action = await dialog.ShowAsync();
            return action?.Id;
        }

        public async Task<object> ShowDialogOnContextDispatcher(MessageDialog dialog)
        {
            object actionId = null;

            await context.Dispatcher.RunAsync(
                CoreDispatcherPriority.Normal,
                async () =>
                {
                    var result = await dialog.ShowAsync();
                    actionId = result?.Id;
                }
            );

            return actionId;
        }

        public async Task<bool> DisplayAlert(string title, string message, string accept, string cancel)
        {
            var dialog = new MessageDialog(message, title);

            if (cancel != null || accept != null)
            {
                var userActions = accept == null ? new UserAction[0] : new[] {new UserAction(accept)};
                AddDialogActions(dialog, cancel, userActions);
            }

            var choice = await ShowDialog(dialog);
            return choice?.Equals(accept) ?? false;
        }

        public Task DisplayException(string title, Exception exception)
        {
            var message = Debugger.IsAttached ? exception.ToString() : exception.Message;
            return DisplayAlert(title, message, null, null);
        }

        public async Task<string> DisplayPrompt(string title, string message, string value, string accept, string cancel)
        {
            var textBox = new TextBox { Text = value };

            var view = new Grid
            {
                Children =
                {
                    new StackPanel
                    {
                        Orientation = Orientation.Vertical,
                        Children =
                        {
                            new TextBlock { Text = message },
                            textBox
                        }
                    }
                }
            };

            var acceptAction = new UserAction(accept);
            var cancelAction = new UserAction(cancel);

            return await DisplayDialog(view, acceptAction, cancelAction) == acceptAction ? textBox.Text : null;
        }

        public async Task<UserAction> DisplayDialog(
            object viewModel, 
            UserAction primaryAction,
            UserAction secondaryAction, 
            CancellationToken cancellationToken)
        {
            var view = viewRegistry.GetViewFor(viewModel) as UIElement;

            if (viewModel is INavigationDestination target)
            {
                await target.NavigateTo();
            }

            return await DisplayDialog(view, primaryAction, secondaryAction,
                dialog =>
                {
                    dialog.SetBinding(ContentDialog.IsPrimaryButtonEnabledProperty, new Binding
                    {
                        Source = viewModel,
                        Path = new PropertyPath("IsValid"),
                        Mode = BindingMode.OneWay
                    });
                },
                cancellationToken
            );
        }

        private static async Task<UserAction> DisplayDialog(
            UIElement view,
            UserAction primaryAction, 
            UserAction secondaryAction,
            Action<ContentDialog> initializer = null, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var dialog = CreateDialog(view, primaryAction, secondaryAction);
            cancellationToken.Register(() => dialog.Hide());
            initializer?.Invoke(dialog);

            switch (await dialog.ShowAsync())
            {
                case ContentDialogResult.Primary:
                    return primaryAction;
                case ContentDialogResult.Secondary:
                    return secondaryAction;
                default:
                    return null;
            }
        }

        private static ContentDialog CreateDialog(UIElement view, UserAction primaryAction, UserAction secondaryAction)
        {
            var dialog = new ContentDialog
            {
                Content = view,
                Background = (Brush) Application.Current.Resources["ApplicationPageBackgroundThemeBrush"]
            };

            if (primaryAction != null)
            {
                dialog.IsPrimaryButtonEnabled = true;
                dialog.PrimaryButtonText = primaryAction.Name;
                dialog.PrimaryButtonCommand = GetActionCommand(primaryAction);
                dialog.PrimaryButtonCommandParameter = primaryAction.Argument;
            }

            if (secondaryAction != null)
            {
                dialog.IsSecondaryButtonEnabled = true;
                dialog.SecondaryButtonText = secondaryAction.Name;
                dialog.SecondaryButtonCommand = GetActionCommand(secondaryAction);
                dialog.SecondaryButtonCommandParameter = secondaryAction.Argument;
            }

            return dialog;
        }

        private static UIElement ConstrainToScreen(UIElement view)
        {
            if (DeviceFormFactorTrigger.GetFormFactor() == DeviceFormFactor.Phone)
                return view;

            var element = view as FrameworkElement;
            if (element == null)
                return view;

            element.MaxHeight = 600;
            return view;
        }

        private static UIElement MakeScrollable(UIElement view)
        {
            if (view.FindVisualChild<ScrollViewer>() != null)
                return view;

            if (view.FindVisualChild<ListView>() != null)
                return view;

            return new ScrollViewer
            {
                Content = view,
                VerticalScrollMode = ScrollMode.Auto,
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                HorizontalScrollMode = ScrollMode.Disabled,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled
            };
        }

        private static ICommand GetActionCommand(UserAction action)
        {
            return action.Command ?? (action.Action != null ? new Command(action.Action) : null);
        }

        public async Task<UserAction> SelectAction(string title, string cancel, IEnumerable<UserAction> actions)
        {
            var dialog = new MessageDialog(title);
            var actionArray = actions?.ToArray();
            AddDialogActions(dialog, cancel, actionArray);
            var choice = await ShowDialog(dialog);
            return actionArray.FirstOrDefault(a => a.Name.Equals(choice));
        }

        private static void AddDialogActions(MessageDialog dialog, string cancel, IEnumerable<UserAction> actions)
        {
            foreach (var action in actions)
            {
                dialog.Commands.Add(new UICommand(action.Name, _ => action.Execute(), action.Name));
            }

            if (cancel == null) // || ApiInformation.IsApiContractPresent("Windows.Phone.PhoneContract", 1)) return;
                return;

            dialog.Commands.Add(new UICommand(cancel, _ => {}, cancel));
            dialog.CancelCommandIndex = (uint)(dialog.Commands.Count - 1);
        }
    }
}
