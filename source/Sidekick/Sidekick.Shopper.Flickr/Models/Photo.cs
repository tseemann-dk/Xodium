namespace Sidekick.Shopper.Flickr.Models
{
    class Photo
    {
        public string Id { get; set; }
        public string Owner { get; set; }
        public string Secret { get; set; }
        public int Server { get; set; }
        public int Farm { get; set; }
        public string Title { get; set; }

        public string ThumbnailUrl => $"https://farm{Farm}.staticflickr.com/{Server}/{Id}_{Secret}_q.jpg";
    }
}
