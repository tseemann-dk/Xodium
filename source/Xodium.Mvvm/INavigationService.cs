using System;
using System.Threading.Tasks;

namespace Xodium.Mvvm
{
    public interface INavigationService
    {
        bool CanGoBack { get; }
        bool IsAtRoot { get; }
        Task GoBack();
        Task GoBack(int count);
        Task GoBackToRoot();
        Task GoTo(object viewModel);
        Task GoTo(Type viewModelType);
        Task OpenModal(object viewModel);
        Task OpenPopup(object viewModel);
        Task OpenUri(Uri uri);
        Task RestartAt(object viewModel);
        Task RestartAt(Type viewModelType);
    }
}
