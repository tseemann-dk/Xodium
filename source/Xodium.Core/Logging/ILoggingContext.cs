using System.Threading.Tasks;

namespace Xodium.Logging
{
    public interface ILoggingContext
    {
        Task<ILoggingScope> CreateScope();
    }
}
