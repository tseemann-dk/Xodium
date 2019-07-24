using System;

namespace Xodium.Mvvm
{
    public interface IViewRegistry
    {
        object GetViewFor(object viewModel);
        object GetViewFor(Type viewModelType);
        Type GetViewTypeFor(Type viewModelType);

        void RegisterViewType<TView, TViewModel>();
        void RegisterViewType(Type viewType, Type viewModelType);
        void RegisterViewFactory<TViewModel>(Func<TViewModel, object> viewProvider);
        void RegisterViewFactory(Type viewModelType, Func<object, object> viewProvider);
    }
}