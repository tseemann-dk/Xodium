using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Xodium.Serialization.Json.CoreFX
{
    public class NativeJsonSerializer : ISerializer
    {
        private readonly JsonSerializerOptions options;

        public NativeJsonSerializer(JsonSerializerOptions options)
        {
            this.options = options ?? new JsonSerializerOptions();
        }

        public T Deserialize<T>(Stream stream) =>
            JsonSerializer.Deserialize<T>(stream, options);

        public ValueTask<T> DeserializeAsync<T>(Stream stream, CancellationToken cancellationToken) =>
            JsonSerializer.DeserializeAsync<T>(stream, options);

        public void Serialize<T>(T value, Stream stream) =>
            JsonSerializer.Serialize(stream, value, options);

        public Task SerializeAsync<T>(T value, Stream stream, CancellationToken cancellationToken) =>
            JsonSerializer.SerializeAsync(stream, value, options);
    }
}
