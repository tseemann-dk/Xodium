using Newtonsoft.Json;

namespace Sidekick.Shopper.Flickr.Models
{
    class PhotoCollection
    {
        public int Page { get; set; }
        public int Pages { get; set; }
        public int PerPage { get; set; }
        public int Total { get; set; }

        [JsonProperty("photo")]
        public Photo[] Items { get; set; }
    }
}
