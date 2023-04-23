using System.Threading.Tasks;

namespace Xodium.Logging
{
    public interface ILogger
    {
        void Log(string message);
        Task LogAsync(string message);
    }
}
