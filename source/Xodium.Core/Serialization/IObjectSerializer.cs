using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Xodium.Serialization
{
    public interface IObjectSerializer
    {
        Task Serialize<T>(T obj, Stream stream, CancellationToken cancellationToken = default);
        Task<T> Deserialize<T>(Stream stream, CancellationToken cancellationToken = default);
    }
}
