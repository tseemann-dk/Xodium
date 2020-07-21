using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xodium.Platform.Xamarin.Services
{
    public interface IPopupService
    {
        IReadOnlyCollection<Page> PopupStack { get; }

        bool CanShowPage(Page page);
        Task PopAllPages(bool animate = true);
        Task PopPage(bool animate = true);
        Task PushPage(Page page, bool animate = true);
    }
}
