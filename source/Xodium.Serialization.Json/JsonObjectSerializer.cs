using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace Xodium.Serialization.Json
{
    public class JsonObjectSerializer : IObjectSerializer
    {
        public async Task Serialize<T>(T obj, Stream stream)
        {
            using (var streamWriter = new StreamWriter(stream))
            {
                var serializer = JsonSerializer.CreateDefault();

                using (var jsonWriter = new JsonTextWriter(streamWriter))
                {
                    serializer.Serialize(jsonWriter, obj);
                    await streamWriter.FlushAsync().ConfigureAwait(false);
                    stream.Position = 0;
                }
            }
        }

        public Task<T> Deserialize<T>(Stream stream)
        {
            using (var streamReader = new StreamReader(stream))
            {
                using (var jsonReader = new JsonTextReader(streamReader))
                {
                    var serializer = JsonSerializer.CreateDefault();
                    var obj = serializer.Deserialize<T>(jsonReader);

                    return Task.FromResult(obj);
                }
            }
        }
    }
}
