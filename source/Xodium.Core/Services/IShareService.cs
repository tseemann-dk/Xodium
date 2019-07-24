using System;
using System.Threading.Tasks;

namespace Xodium.Services
{
    public interface IShareService
    {
        Task ShareLink(Uri uri, string title);
        Task ShareText(string text, string title);
    }
}
