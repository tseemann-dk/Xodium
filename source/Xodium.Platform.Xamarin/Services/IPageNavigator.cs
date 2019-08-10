using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xodium.Platform.Xamarin.Services
{
    public interface IPageNavigator
    {
        IEnumerable<Page> Pages { get; }
        Page FirstPage { get; }
        Page LastPage { get; }
        int PageCount { get; }
        bool CanGoBack { get; }
        bool IsAtRoot { get; }
        bool CanGoTo(Page page);
        Task GoTo(Page page);
        Task<Page> GoBack(bool animated);
        Task Reset();
        Task ResetTo(Page page);
        Task ResetToRoot();
    }
}
