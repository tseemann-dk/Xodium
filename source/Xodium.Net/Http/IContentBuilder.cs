using System.Net.Http;
using System.Threading.Tasks;

namespace Xodium.Net.Http
{
    public interface IContentBuilder
    {
        Task<HttpContent> BuildContent();
    }
}
