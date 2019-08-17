using Newtonsoft.Json;

namespace Sidekick.Features.Shopper.Models
{
    public interface IShoppingItem : IShoppingNode
    {
        string ComponentNumber { get; }
        [JsonIgnore] IComponent Component { get; }
    }
}
