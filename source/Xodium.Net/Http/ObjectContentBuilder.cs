using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xodium.Serialization;

namespace Xodium.Net.Http
{
    public class ObjectContentBuilder<T> : IContentBuilder
    {
        private readonly T value;
        private readonly IObjectSerializer serializer;
        private readonly string mediaType;

        public ObjectContentBuilder(T value, IObjectSerializer serializer, string mediaType)
        {
            this.value = value;
            this.serializer = serializer;
            this.mediaType = mediaType;
        }

        public async Task<HttpContent> BuildContent()
        {
            var stream = new MemoryStream();
            await serializer.SerializeAsync(stream, value); 
            await stream.FlushAsync();
            stream.Position = 0;
            var content = new StreamContent(stream);
            content.Headers.ContentType = new MediaTypeHeaderValue(mediaType);
            return content;
        }
    }
}
