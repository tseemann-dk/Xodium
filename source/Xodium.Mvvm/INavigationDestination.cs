using System.Threading.Tasks;

namespace Xodium.Mvvm
{
    public interface INavigationDestination
    {
        Task NavigateBackFrom();
        Task NavigateBackTo();
        Task NavigateFrom();
        Task NavigateTo();
    }
}