using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Xodium.Mvvm;

namespace Xodium.Platform.Uno.Services
{
    public class NavigationService : INavigationService
    {
        private readonly Lazy<Frame> frame;
        private readonly Lazy<IViewRegistry> viewRegistry;
        private readonly IDialogService dialogService;

        public NavigationService(Func<Frame> frameProvider, Func<IViewRegistry> viewRegistryProvider, IDialogService dialogService)
        {
            if (frameProvider is null)
                throw new ArgumentNullException(nameof(frameProvider));
            
            if (viewRegistryProvider is null)
                throw new ArgumentNullException(nameof(viewRegistryProvider));

            this.dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));

            frame = new Lazy<Frame>(() => frameProvider());
            viewRegistry = new Lazy<IViewRegistry>(() => viewRegistryProvider());
        }

        public bool CanGoBack => Frame.CanGoBack;
        public bool IsAtRoot => !Frame.CanGoBack;

        protected Frame Frame => frame.Value ?? throw new NotSupportedException("Frame not assigned");
        protected IViewRegistry ViewRegistry => viewRegistry.Value ?? throw new NotSupportedException("ViewRegistry not assigned");

        public Task GoBack()
        {
            Frame.GoBack();
            return Task.CompletedTask;
        }

        public Task GoBack(int count)
        {
            while (count-- > 0)
            {
                Frame.GoBack();
            }

            return Task.CompletedTask;
        }

        public Task GoBackToRoot()
        {
            while (Frame.CanGoBack)
            {
                Frame.GoBack();
            }

            return Task.CompletedTask;
        }

        public Task GoTo(object viewModel)
        {
            if (viewModel is null)
                throw new ArgumentNullException(nameof(viewModel));

            var viewType = ViewRegistry.GetViewTypeFor(viewModel.GetType());
            Frame.Navigate(viewType, viewModel);
            return Task.CompletedTask;
        }

        public Task GoTo(Type viewModelType)
        {
            if (viewModelType is null)
                throw new ArgumentNullException(nameof(viewModelType));

            var viewType = ViewRegistry.GetViewTypeFor(viewModelType);
            Frame.Navigate(viewType);
            return Task.CompletedTask;
        }

        public Task OpenModal(object viewModel)
        {
            return dialogService.DisplayDialog(viewModel);
        }

        public Task OpenPopup(object viewModel)
        {
            return dialogService.DisplayDialog(viewModel);
        }

        public Task OpenUri(Uri uri)
        {
            throw new NotImplementedException();
        }

        public async Task RestartAt(object viewModel)
        {
            await GoBackToRoot();
            await GoTo(viewModel);
        }

        public async Task RestartAt(Type viewModelType)
        {
            await GoBackToRoot();
            await GoTo(viewModelType);
        }
    }
}
