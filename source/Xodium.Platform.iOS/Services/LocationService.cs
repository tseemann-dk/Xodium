using System;
using System.Threading;
using System.Threading.Tasks;
using Xodium.Geography;
using Xodium.Services;

// TODO: Add implementation

namespace Xodium.Platform.iOS.Services
{
    public class LocationService : ILocationService
    {
        public Task<TrackedPosition> GetCurrentPosition(TimeSpan maximumAge, TimeSpan timeout, CancellationToken cancellationToken)
        {
            return Task.FromResult(TrackedPosition.Empty);
        }

        public ILocationListener GetListener(LocationListenerSettings settings)
        {
            return new LocationListener(settings ?? LocationListenerSettings.Default);
        }
    }

    internal class LocationListener : ILocationListener
    {
        private readonly LocationListenerSettings settings;

        public LocationListener(LocationListenerSettings settings)
        {
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        public bool IsListening => false;
        public TrackedPosition LastKnownPosition => TrackedPosition.Empty;

        public event EventHandler<PositionChangedEventArgs> PositionChanged;

        public Task Start()
        {
            return Task.CompletedTask;
        }

        public Task Stop()
        {
            return Task.CompletedTask;
        }
    }
}
