using Newtonsoft.Json;

namespace Sidekick.Models
{
    public interface IShortcut : IArchiveNode
    {
        [JsonIgnore] IElement Target { get; }
    }
}
