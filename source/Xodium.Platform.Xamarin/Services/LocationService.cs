using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Plugin.Geolocator.Abstractions;
using Xodium.Geography;
using Xodium.Services;

namespace Xodium.Platform.Xamarin.Services
{
    public static class PositionExtensions
    {
        public static TrackedPosition ToTrackedPosition(this Position position) => new TrackedPosition
        {
            Position = new GeoPosition(position.Latitude, position.Longitude, position.Altitude),
            Heading = position.Heading,
            Speed = position.Speed
        };
    }

    public class LocationService : ILocationService
    {
        private readonly IGeolocator locator;

        public LocationService()
        {
            locator = Plugin.Geolocator.CrossGeolocator.Current;
        }

        public async Task<TrackedPosition> GetCurrentPosition(TimeSpan maximumAge, TimeSpan timeout, CancellationToken cancellationToken = default(CancellationToken))
        {
            return (await RetrieveCurrentPosition(timeout, cancellationToken))?.ToTrackedPosition();
        }

        public ILocationListener GetListener(LocationListenerSettings settings)
        {
            // NB! The library Xam.Plugin.Geolocator uses a singleton instance for all location awareness, 
            // including position listening. All listeners returned by this location service therefore share
            // the same underlying locator instance. This means that only one listener can be active at any 
            // given time. Attempting to start one listener when another is already running will result in
            // an exception being thrown.

            return new LocationListener(locator, settings ?? LocationListenerSettings.Default);
        }

        private async Task<Position> RetrieveCurrentPosition(TimeSpan timeout, CancellationToken cancellationToken)
        {
            try
            {
                var position = await locator.GetLastKnownLocationAsync();

                if (position != null)
                {
                    return position;
                }

                if (!locator.IsGeolocationAvailable || !locator.IsGeolocationEnabled)
                {
                    return null;
                }

                return await locator.GetPositionAsync(timeout, cancellationToken, true);
            }
            catch (TaskCanceledException)
            {
                return null;
            }
            catch (Exception exception)
            {
                Debug.WriteLine("Error getting position: " + exception.Message);
                return null;
            }
        }
    }

    internal class LocationListener : ILocationListener
    {
        private readonly IGeolocator locator;
        private readonly LocationListenerSettings settings;

        public LocationListener(IGeolocator locator, LocationListenerSettings settings)
        {
            this.locator = locator ?? throw new ArgumentNullException(nameof(locator));
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        public bool IsListening => locator.IsListening;
        public TrackedPosition LastKnownPosition { get; private set; }

        public event EventHandler<PositionChangedEventArgs> PositionChanged;

        public Task Start()
        {
            if (locator.IsListening)
                throw new InvalidOperationException("Locator is already listening");

            locator.DesiredAccuracy = 1;
            locator.PositionChanged += Locator_PositionChanged;

            return locator.StartListeningAsync(settings.MinimumTime, settings.MinimumDistance.Meters, true,
                new ListenerSettings
                {
                    ActivityType = ActivityType.Other,
                    AllowBackgroundUpdates = true,
                    DeferLocationUpdates = true,
                    DeferralDistanceMeters = 1,
                    DeferralTime = TimeSpan.FromSeconds(1),
                    ListenForSignificantChanges = true,
                    PauseLocationUpdatesAutomatically = false
                }
            );
        }

        public Task Stop()
        {
            return locator.StopListeningAsync();
        }

        private void Locator_PositionChanged(object sender, PositionEventArgs e)
        {
            LastKnownPosition = e.Position.ToTrackedPosition();
            PositionChanged?.Invoke(this, new PositionChangedEventArgs(LastKnownPosition));
        }
    }
}
