using ReactiveUI;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Xodium.Mvvm.ReactiveUI.Services
{
    public class NavigationService : ReactiveObject, INavigationService
    {
        private readonly IScreen screen;

        public NavigationService(IScreen screen)
        {
            this.screen = screen ?? throw new ArgumentNullException(nameof(screen));

            //this.WhenAnyValue(x => x.Router.NavigateBack.CanExecute)
            //    .ToProperty(this, x => x.CanGoBack, out canGoBack);
        }

        protected RoutingState Router => screen.Router;

        //private readonly ObservableAsPropertyHelper<bool> canGoBack;
        public bool CanGoBack => false; // canGoBack.Value;

        public bool IsAtRoot => !Router.NavigationStack.Any();

        public Task GoBack()
        {
            throw new NotImplementedException();
            //return Router.NavigateBack.Execute();
        }

        public Task GoBack(int count)
        {
            throw new NotImplementedException();
        }

        public Task GoBackToRoot()
        {
            throw new NotImplementedException();
        }

        public Task GoTo(object viewModel)
        {
            throw new NotImplementedException();
        }

        public Task GoTo(Type viewModelType)
        {
            throw new NotImplementedException();
        }

        public Task OpenModal(object viewModel)
        {
            throw new NotImplementedException();
        }

        public Task OpenPopup(object viewModel)
        {
            throw new NotImplementedException();
        }

        public Task OpenUri(Uri uri)
        {
            throw new NotImplementedException();
        }

        public Task RestartAt(object viewModel)
        {
            throw new NotImplementedException();
        }

        public Task RestartAt(Type viewModelType)
        {
            throw new NotImplementedException();
        }
    }
}
