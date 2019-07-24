using System.Threading.Tasks;

namespace Xodium.Mvvm
{
    public interface INavigationTarget
    {
        Task NavigateBackFrom();
        Task NavigateBackTo();
        Task NavigateFrom();
        Task NavigateTo();
    }
}