using System;
using System.Linq;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xodium.Mvvm;
using Xodium.Platform.Xamarin.Extensions;
using Xodium.Platform.Xamarin.Views;

namespace Xodium.Platform.Xamarin.Services
{
    public class NavigationService : INavigationService
    {
        private INavigationTarget currentPlace;
        private readonly Func<IViewRegistry> getViewRegistry;
        private readonly IPageNavigator regularPageNavigator;
        private readonly IPageNavigator modalPageNavigator;
        private readonly IPageNavigator popupPageNavigator;
        private readonly IPageNavigator pageNavigator;

        #region Construction

        public NavigationService(NavigationPage page, Func<IViewRegistry> getViewRegistry)
            : this(page?.Navigation, PopupNavigation.Instance, getViewRegistry)
        {
            page.Popped += async (s, e) => await OnPagePopped(e.Page);
        }

        public NavigationService(INavigation basicNavigation, IPopupNavigation popupNavigation, Func<IViewRegistry> getViewRegistry)
        {
            if (basicNavigation == null)
            {
                throw new ArgumentNullException(nameof(basicNavigation));
            }

            if (popupNavigation == null)
            {
                throw new ArgumentNullException(nameof(popupNavigation));
            }

            this.getViewRegistry = getViewRegistry ?? throw new ArgumentNullException(nameof(getViewRegistry));

            pageNavigator = new CompositePageNavigator(new[] {
                regularPageNavigator = new RegularPageNavigator(basicNavigation),
                modalPageNavigator = new ModalPageNavigator(basicNavigation),
                popupPageNavigator = new PopupPageNavigator(popupNavigation, page => OnPagePopped(page))
            });
        }

        #endregion

        #region Attached Properties

        public static readonly BindableProperty PageTitleProperty = BindableProperty.CreateAttached(
            "PageTitle", typeof(string), typeof(NavigationService), default(string));

        public static string GetPageTitle(BindableObject bindable) => (string)bindable.GetValue(PageTitleProperty);
        public static void SetPageTitle(BindableObject bindable, string value) => bindable.SetValue(PageTitleProperty, value);

        #endregion

        #region Basic Properties

        public Page CurrentPage => pageNavigator.LastPage;
        public bool CanGoBack => pageNavigator.CanGoBack;
        public bool IsAtRoot => pageNavigator.IsAtRoot;

        #endregion

        #region Public Methods

        public Task GoTo(object viewModel) => NavigateTo(viewModel, restart: false);
        public Task GoTo(Type viewModelType) => NavigateTo(viewModelType, restart: false);
        public Task GoBack() => NavigateBack(1);
        public Task GoBack(int count) => NavigateBack(count);
        public Task GoBackToRoot() => NavigateBackToRoot();
        public Task OpenModal(object viewModel) => NavigateToModalView(GetViewForViewModel(viewModel), viewModel);
        public Task OpenPopup(object viewModel) => NavigateToPopupView(GetViewForViewModel(viewModel), viewModel);
        public Task RestartAt(object viewModel) => NavigateTo(viewModel, restart: true);
        public Task RestartAt(Type viewModelType) => NavigateTo(viewModelType, restart: true);

        public Task OpenUri(Uri uri)
        {
            Device.OpenUri(uri);
            return Task.CompletedTask;
        }

        #endregion

        #region Internal Navigation

        private Task NavigateTo(Type viewModelType, bool restart)
        {
            var view = GetViewForViewModelType(viewModelType) as BindableObject;
            var viewModel = view?.BindingContext;
            return NavigateToView(view, viewModel, restart);
        }

        private Task NavigateTo(object viewModel, bool restart)
        {
            return NavigateToView(GetViewForViewModel(viewModel), viewModel, restart);
        }

        private Task NavigateToView(object target, object viewModel, bool restart)
        {
            var page = EnsureAndPreparePage(target, viewModel);
            return restart ? RestartAtPage(page) : NavigateToPage(page);
        }

        private Task NavigateToModalView(object target, object viewModel)
        {
            return NavigateToModalPage(EnsureAndPreparePage(target, viewModel));
        }

        private Task NavigateToPopupView(object target, object viewModel)
        {
            return NavigateToPopupPage(EnsureAndPreparePopupPage(target, viewModel));
        }

        private async Task NavigateToPage(Page page)
        {
            await OnNavigatingToPage(page);
            await regularPageNavigator.GoTo(page);
        }

        private async Task RestartAtPage(Page page)
        {
            await pageNavigator.Reset();
            await NavigateToPage(page);
        }

        private async Task NavigateToModalPage(Page page)
        {
            await OnNavigatingToPage(page);
            await modalPageNavigator.GoTo(page);
        }

        private async Task NavigateToPopupPage(PopupPage page)
        {
            await OnNavigatingToPage(page);
            page.Disappearing += (s, e) => OnPageDismissed(page);
            await popupPageNavigator.GoTo(page);
        }

        private async Task NavigateBack(int count)
        {
            if (count <= 0) return;

            await NavigateBack(true);

            while (--count > 0)
            {
                await NavigateBack(false);
            }
        }

        private Task<Page> NavigateBack(bool animated)
        {
            return pageNavigator.GoBack(animated);
        }

        private Task NavigateBackToRoot()
        {
            return pageNavigator.ResetToRoot();
        }

        private Task OnNavigatingToPage(Page page)
        {
            return OnNavigatingToViewModel(page?.BindingContext);
        }

        private Task OnNavigatingBackToPage(Page page)
        {
            return OnNavigatingBackToViewModel(page?.BindingContext);
        }

        private async Task OnNavigatingToViewModel(object viewModel)
        {
            if (currentPlace != null)
            {
                await currentPlace.NavigateFrom();
            }

            currentPlace = viewModel as INavigationTarget;

            if (currentPlace != null)
            {
                await currentPlace.NavigateTo();
            }
        }

        private async Task OnNavigatingBackToViewModel(object viewModel)
        {
            if (currentPlace != null)
            {
                await currentPlace.NavigateBackFrom();
            }

            currentPlace = viewModel as INavigationTarget;

            if (currentPlace != null)
            {
                await currentPlace.NavigateBackTo();
            }
        }

        #endregion

        #region Appearing/Disappearing

        private void OnPageAppearing(Page page)
        {
            if (!(page is ContentPage contentPage)) return;

            var content = contentPage.Content;

            if (content is IAppearable appearable)
            {
                appearable.OnAppearing();
            }

            foreach (var child in content.GetAllChildren().OfType<IAppearable>())
            {
                child.OnAppearing();
            }
        }

        private void OnPageDisappearing(Page page)
        {
            if (!(page is ContentPage contentPage)) return;

            var content = contentPage.Content;

            if (content is IAppearable appearable)
            {
                appearable.OnDisappearing();
            }

            foreach (var child in content.GetAllChildren().OfType<IAppearable>())
            {
                child.OnDisappearing();
            }
        }

        #endregion

        #region Page Dismission

        public async Task OnPagePopped(Page page)
        {
            try
            {
                await OnPageDismissed(page);
            }
            finally
            {
                await OnNavigatingBackToPage(CurrentPage);
            }
        }

        protected virtual Task OnPageDismissed(Page page)
        {
            page.BindingContext = null;
            return Task.CompletedTask;
        }

        #endregion

        #region Page Factory Methods

        private Page EnsureAndPreparePage(object content, object viewModel)
        {
            return PreparePage(EnsurePage(content, viewModel));
        }

        private PopupPage EnsureAndPreparePopupPage(object content, object viewModel)
        {
            return PreparePage(EnsurePopupPage(content, viewModel)) as PopupPage;
        }

        private Page EnsurePage(object content, object viewModel)
        {
            switch (content)
            {
                case Page page:
                    return page;
                case View view:
                    return CreatePage(view, viewModel);
                default:
                    throw new ArgumentException($"Cannot create page for content of type {content.GetType().FullName}");
            }
        }

        private PopupPage EnsurePopupPage(object content, object viewModel)
        {
            switch (content)
            {
                case PopupPage page:
                    return page;
                case View view:
                    return CreatePopupPage(view, viewModel);
                default:
                    throw new ArgumentException($"Cannot create popup page for content of type {content.GetType().FullName}");
            }
        }

        private Page PreparePage(Page page, object viewModel = null)
        {
            if (page == null) return null;
            page.Appearing += (s, e) => OnPageAppearing(page);
            page.Disappearing += (s, e) => OnPageDisappearing(page);
            page.BindingContext = viewModel ?? page.BindingContext;
            return page;
        }

        protected virtual Page CreatePage(View view, object viewModel)
        {
            return new ContentPage
            {
                BindingContext = viewModel,
                Content = CreatePageContent(view),
                Title = GetTitleForView(view)
            };
        }

        protected virtual string GetTitleForView(View view)
        {
            return GetPageTitle(view) ?? view.GetType().Name;
        }

        protected virtual View CreatePageContent(View view)
        {
            return view;
        }

        protected virtual PopupPage CreatePopupPage(View view, object viewModel)
        {
            return new PopupPage
            {
                BindingContext = viewModel,
                Content = CreatePopupView(view),
                CloseWhenBackgroundIsClicked = false
            };
        }

        protected virtual View CreatePopupView(View content)
        {
            return CreateDefaultPopupView(content);
        }

        protected View CreateDefaultPopupView(View content)
        {
            return new PopupView { Content = content };
        }

        #endregion

        #region View Lookup Methods

        private object GetViewForViewModelType(Type viewModelType)
        {
            if (viewModelType == null)
            {
                throw new ArgumentNullException(nameof(viewModelType));
            }

            return getViewRegistry()?.GetViewFor(viewModelType) ?? throw new NavigationException($"No view registered for {viewModelType.FullName}");
        }

        private object GetViewForViewModel(object viewModel)
        {
            if (viewModel == null)
            {
                throw new ArgumentNullException(nameof(viewModel));
            }

            var view = getViewRegistry()?.GetViewFor(viewModel);

            if (view == null)
            {
                throw new NavigationException($"No view registered for {viewModel.GetType().FullName}");
            }

            if (view is BindableObject b && b.BindingContext == null)
            {
                b.BindingContext = viewModel;
            }

            return view;
        }

        #endregion
    }
}
