using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Xodium.Serialization.Json.Newtonsoft
{
    public class JsonObjectSerializer : IObjectSerializer
    {
        private readonly JsonSerializer serializer;

        public JsonObjectSerializer(JsonSerializer serializer = null)
        {
            this.serializer = serializer ?? JsonSerializer.CreateDefault();
        }

        public void Serialize<T>(Stream stream, T value)
        {
            using var streamWriter = new StreamWriter(stream, Encoding.UTF8, 1024, true);
            using var jsonWriter = new JsonTextWriter(streamWriter);
            serializer.Serialize(jsonWriter, value);
            streamWriter.Flush();
            stream.Position = 0;
        }

        public async Task SerializeAsync<T>(Stream stream, T value, CancellationToken cancellationToken)
        {
            using var streamWriter = new StreamWriter(stream, Encoding.UTF8, 1024, true);
            using var jsonWriter = new JsonTextWriter(streamWriter);
            serializer.Serialize(jsonWriter, value);
            await streamWriter.FlushAsync().ConfigureAwait(false);
            stream.Position = 0;
        }

        public T Deserialize<T>(Stream stream)
        {
            using var streamReader = new StreamReader(stream, Encoding.UTF8, true, 1024, true);
            using var jsonReader = new JsonTextReader(streamReader);
            var value = serializer.Deserialize<T>(jsonReader);
            return value;
        }

        public ValueTask<T> DeserializeAsync<T>(Stream stream, CancellationToken cancellationToken)
        {
            var value = Deserialize<T>(stream);
            return new ValueTask<T>(value);
        }
    }
}
