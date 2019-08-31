using Newtonsoft.Json;

namespace Sidekick.Shopper.Models
{
    public interface IShoppingItem : IShoppingNode
    {
        string ComponentNumber { get; }
        [JsonIgnore] IComponent Component { get; }
    }
}
