using System;
using System.Threading;
using System.Threading.Tasks;
using Xodium.Geography;

namespace Xodium.Services
{
    public interface ILocationService
    {
        Task<TrackedPosition> GetCurrentPosition(TimeSpan maximumAge, TimeSpan timeout, CancellationToken cancellationToken = default);
        ILocationListener GetListener(LocationListenerSettings settings = null);
    }
}
