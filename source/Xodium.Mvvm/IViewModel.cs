using System.Collections.Generic;
using System.Threading.Tasks;

namespace Xodium.Mvvm
{
    public interface IViewModel : INavigationSource, INavigationTarget
    {
        IParentViewModel ParentViewModel { get; }

        Task OnNavigateTo(NavigationDirection direction);
        Task OnNavigateFrom(NavigationDirection direction);
    }

    public interface IParentViewModel : IViewModel
    {
        IReadOnlyList<IViewModel> ChildViewModels { get; }

        void AddChild(IViewModel child);
    }
}
