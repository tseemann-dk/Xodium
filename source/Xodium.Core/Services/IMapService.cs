using System.Threading.Tasks;
using Xodium.Geography;

namespace Xodium.Services
{
    public interface IMapService
    {
        Task<string> GetAddressFromPosition(GeoPosition position);
        Task<GeoPosition> GetPositionFromAddress(string address);
        Task<bool> ProvideDirections(GeoPosition destination, string destinationName = null, GeoPosition source = null, string sourceName = null);
    }
}
