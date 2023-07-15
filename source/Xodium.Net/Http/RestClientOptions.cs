using Xodium.Serialization;

namespace Xodium.Net.Http
{
    public class RestClientOptions
    {
        public ISerializer Serializer { get; set; }
        public string DefaultResponseMediaType { get; set; }
        public string DefaultRequestMediaType { get; set; }

        public static RestClientOptions CreateForJsonSerializer(ISerializer serializer) => new RestClientOptions()
        {
            Serializer = serializer,
            DefaultRequestMediaType = "application/json",
            DefaultResponseMediaType = "application/json",
        };
    }
}
