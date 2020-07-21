using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using Xodium.Mvvm;

// TODO: Finish implementation

namespace Xodium.Platform.Windows.Wpf.Services
{
    public class NavigationService : INavigationService
    {
        private readonly Stack<Window> modalStack = new Stack<Window>();
        private readonly Func<IViewRegistry> getViewRegistry;

        public NavigationService(Func<IViewRegistry> getViewRegistry)
        {
            this.getViewRegistry = getViewRegistry ?? throw new ArgumentNullException(nameof(getViewRegistry));
        }

        public static readonly DependencyProperty PageTitleProperty = DependencyProperty
            .RegisterAttached("PageTitle", typeof(string), typeof(NavigationService), new PropertyMetadata(null));

        public static string GetPageTitle(DependencyObject obj) => (string)obj.GetValue(PageTitleProperty);
        public static void SetPageTitle(DependencyObject obj, string value) => obj.SetValue(PageTitleProperty, value);

        public bool CanGoBack => throw new NotImplementedException();
        public bool IsAtRoot => throw new NotImplementedException();

        public Task GoBack()
        {
            return Task.CompletedTask;
        }

        public Task GoBack(int count)
        {
            if (modalStack.Peek() is Window window)
            {
                window.Close();
            }

            return Task.CompletedTask;
        }

        public Task GoBackToRoot()
        {
            return Task.CompletedTask;
        }

        public Task GoTo(object viewModel)
        {
            throw new NotImplementedException();
        }

        public Task GoTo(Type viewModelType)
        {
            throw new NotImplementedException();
        }

        public virtual async Task OpenModal(object viewModel)
        {
            var view = GetViewForViewModel(viewModel);

            if (!(view is FrameworkElement element)) return;

            if (viewModel is INavigationDestination vm)
            {
                await vm.NavigateTo();
            }

            if (!(element is Window window))
            {
                window = CreateWindowForView(element);
            }

            modalStack.Push(window);
            window.Closing += (s, e) => modalStack.Pop();

            window.ShowDialog();
        }

        protected virtual Window CreateWindowForView(FrameworkElement view)
        {
            var window = new Window
            {
                Title = GetTitleForView(view),
                WindowStyle = WindowStyle.SingleBorderWindow,
                ResizeMode = ResizeMode.NoResize,
                SizeToContent = SizeToContent.WidthAndHeight,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ShowInTaskbar = false,
                Margin = new Thickness(12)
            };

            window.Content = view;

            return window;
        }

        protected virtual string GetTitleForView(FrameworkElement view)
        {
            return GetPageTitle(view) ?? view.GetType().Name;
        }

        private object GetViewForViewModel(object viewModel)
        {
            try
            {
                return getViewRegistry()?.GetViewFor(viewModel) ?? throw new NullReferenceException("View registry is missing");
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
                throw;
            }
        }

        public Task OpenPopup(object viewModel)
        {
            return OpenModal(viewModel);
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
