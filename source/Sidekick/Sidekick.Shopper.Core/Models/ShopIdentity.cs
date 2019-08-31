namespace Sidekick.Shopper.Models
{
    public struct ShopIdentity
    {
        public ShopIdentity(string id)
        {
            Id = id;
        }

        public static ShopIdentity Create(string id) => new ShopIdentity(id);

        public static ShopIdentity Internal { get; } = new ShopIdentity("internal");

        public string Id { get; }
    }
}
