using System.IO;
using System.Threading.Tasks;

namespace Xodium.Serialization
{
    public interface IObjectSerializer
    {
        Task Serialize<T>(T obj, Stream stream);
        Task<T> Deserialize<T>(Stream stream);
    }
}
