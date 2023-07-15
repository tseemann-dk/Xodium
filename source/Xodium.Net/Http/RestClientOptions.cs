using Xodium.Serialization;

namespace Xodium.Net.Http
{
    public class RestClientOptions
    {
        public IObjectSerializer Serializer { get; set; }
        public string DefaultResponseMediaType { get; set; }
        public string DefaultRequestMediaType { get; set; }

        public static RestClientOptions CreateForJsonSerializer(IObjectSerializer serializer) => new RestClientOptions()
        {
            Serializer = serializer,
            DefaultRequestMediaType = "application/json",
            DefaultResponseMediaType = "application/json",
        };
    }
}
