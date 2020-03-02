using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Xodium.Serialization.Json.CoreFX
{
    public class JsonObjectSerializer : IObjectSerializer
    {
        private readonly JsonSerializerOptions options;

        public JsonObjectSerializer(JsonSerializerOptions options)
        {
            this.options = options ?? new JsonSerializerOptions();
        }

        public Task<T> Deserialize<T>(Stream stream, CancellationToken cancellationToken)
        {
            return JsonSerializer.DeserializeAsync<T>(stream, options, cancellationToken).AsTask();
        }

        public Task Serialize<T>(T obj, Stream stream, CancellationToken cancellationToken)
        {
            return JsonSerializer.SerializeAsync<T>(stream, obj, options, cancellationToken);
        }
    }
}
