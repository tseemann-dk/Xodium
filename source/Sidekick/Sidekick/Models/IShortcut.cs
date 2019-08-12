using Newtonsoft.Json;

namespace Sidekick.Models
{
    public interface IShortcut : IArchiveNode
    {
        string ElementNumber { get; }
        [JsonIgnore] IElement Element { get; }
    }
}
