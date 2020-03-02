using Newtonsoft.Json;
using System.IO;
using System.Text;
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

        public async Task Serialize<T>(T obj, Stream stream)
        {
            using (var streamWriter = new StreamWriter(stream, Encoding.UTF8, 1024, true))
            using (var jsonWriter = new JsonTextWriter(streamWriter))
            {
                serializer.Serialize(jsonWriter, obj);
                await streamWriter.FlushAsync().ConfigureAwait(false);
                stream.Position = 0;
            }
        }

        public Task<T> Deserialize<T>(Stream stream)
        {
            using (var streamReader = new StreamReader(stream, Encoding.UTF8, true, 1024, true))
            using (var jsonReader = new JsonTextReader(streamReader))
            {
                var obj = serializer.Deserialize<T>(jsonReader);
                return Task.FromResult(obj);
            }
        }
    }
}
