using System;
using System.Threading.Tasks;

namespace Xodium.Services
{
    public interface IDeepLinkInvoker
    {
        Task<bool> CanInvoke(Uri uri);
        Task Invoke(Uri uri);
    }
}
