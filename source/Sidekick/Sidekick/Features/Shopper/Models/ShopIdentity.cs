namespace Sidekick.Features.Shopper.Models
{
    public struct ShopIdentity
    {
        public ShopIdentity(string id)
        {
            Id = id;
        }

        public static ShopIdentity Internal { get; } = new ShopIdentity("internal");

        public string Id { get; }
    }
}
