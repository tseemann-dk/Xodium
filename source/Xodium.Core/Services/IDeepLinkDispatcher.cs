using System;
using System.Threading.Tasks;

namespace Xodium.Services
{
    public interface IDeepLinkDispatcher
    {
        Task Dispatch(Uri uri);
    }
}
