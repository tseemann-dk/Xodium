using System.Threading.Tasks;

namespace Xodium.Services
{
    public interface IClipboardService
    {
        bool IsClipboardSupported { get; }
        Task CopyText(string text);
    }
}
