using System;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Xaml.Controls;
using Xodium.Mvvm;

namespace Xodium.Platform.Uwp.Services
{
    public class NavigationService : INavigationService
    {
        private readonly Frame frame;
        private readonly IViewRegistry viewRegistry;

        public NavigationService(Frame frame, IViewRegistry viewRegistry)
        {
            this.frame = frame ?? throw new ArgumentNullException(nameof(frame));
            this.viewRegistry = viewRegistry ?? throw new ArgumentNullException(nameof(viewRegistry));
        }

        public bool CanGoBack => frame.CanGoBack;
        public bool IsAtRoot => frame.BackStack.Count == 1;

        public Task GoBack()
        {
            frame.GoBack();
            return Task.CompletedTask;
        }

        public Task GoBack(int count)
        {
            while (count-- > 0)
            {
                frame.GoBack();
            }

            return Task.CompletedTask;
        }

        public async Task GoBackToRoot()
        {
            while (frame.BackStack.Count > 1)
            {
                await GoBack();
            }
        }

        private Task<bool> GoToPage(Type pageType, object parameter)
        {
            return Task.FromResult(frame.Navigate(pageType, parameter));
        }

        public Task GoTo(Type viewModelType)
        {
            var viewType = viewRegistry.GetViewTypeFor(viewModelType);
            return GoToPage(viewType, null);
        }

        public async Task GoTo(object viewModel)
        {
            var viewType = viewRegistry.GetViewTypeFor(viewModel.GetType());

            if (viewModel is INavigationTarget target)
            {
                await target.NavigateTo();
            }

            await GoToPage(viewType, viewModel);
        }

        public Task OpenModal(object viewModel)
        {
            return GoTo(viewModel);
        }

        public Task OpenPopup(object viewModel)
        {
            return GoTo(viewModel);
        }

        public async Task OpenUri(Uri uri)
        {
            await Launcher.LaunchUriAsync(uri);
        }

        public Task RestartAt(object viewModel)
        {
            frame.BackStack.Clear();
            return GoTo(viewModel);
        }

        public Task RestartAt(Type viewModelType)
        {
            frame.BackStack.Clear();
            return GoTo(viewModelType);
        }
    }
}
