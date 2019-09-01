using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sidekick.Shopper.Flickr.Models;
using Sidekick.Shopper.Models;

namespace Sidekick.Shopper.Flickr
{
    public class FlickrShop : IShop
    {
        private const string ApiKey = "acb460b39106f67d06676b03e0a48f91";
        private readonly HttpClient httpClient;

        public FlickrShop()
        {
            httpClient = new HttpClient();
        }

        public ShopIdentity ShopIdentity { get; } = new ShopIdentity("flickr");

        public async Task<IReadOnlyList<IComponentDescriptor>> FindComponents(string searchText)
        {
            var response = await httpClient.GetAsync($"https://www.flickr.com/services/rest/?method=flickr.photos.search&api_key={ApiKey}&text={searchText}&format=json&nojsoncallback=1");

            if (!response.IsSuccessStatusCode)
                throw new KeyNotFoundException($"Search for \"{searchText}\" returned status code {response.StatusCode}");
            
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<PhotoResult>(json);

            return result.Photos.Items
                .Select(x => ComponentDescriptor.Create(
                    ShopIdentity, 
                    $"{x.Id}-{x.Secret}", 
                    x.Id, 
                    x.Title, 
                    x.ThumbnailUrl
                ))
                .ToList();
        }
    }
}
