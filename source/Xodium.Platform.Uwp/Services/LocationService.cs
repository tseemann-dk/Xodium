using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Xodium.Geography;
using Xodium.Services;

namespace Xodium.Platform.Uwp.Services
{
    public class LocationService : ILocationService
    {
        private readonly Geolocator locator;

        public LocationService()
        {
            locator = new Geolocator();
        }

        public async Task<TrackedPosition> GetCurrentPosition(TimeSpan maximumAge, TimeSpan timeout, CancellationToken cancellationToken)
        {
            if (!await RequestAccess())
                return null;

            var position = await locator.GetGeopositionAsync(maximumAge, timeout);
            return position.ToTrackedPosition();
        }

        public ILocationListener GetListener(LocationListenerSettings settings)
        {
            return new LocationListener(settings ?? LocationListenerSettings.Default);
        }

        private static async Task<bool> RequestAccess()
        {
            return (await Geolocator.RequestAccessAsync()) == GeolocationAccessStatus.Allowed;
        }

        /* TODO: Future addition
        public async Task<GeoPosition[]> GetPositionsFromSearchText(string searchText, CancellationToken cancellationToken)
        {
            var result = await MapLocationFinder.FindLocationsAsync(searchText, null);

            if (result.Status != MapLocationFinderStatus.Success)
                return null;

            var location = result.Locations.FirstOrDefault();
            if (location == null)
                return null;

            var point = location.Point;
            return new[] { new GeoPosition(point.Position.Latitude, point.Position.Longitude) };
        }
        */
    }

    internal class LocationListener : ILocationListener
    {
        private readonly Geolocator locator;
        private readonly LocationListenerSettings settings;

        public bool IsListening { get; private set; }
        public TrackedPosition LastKnownPosition { get; private set; }

        public LocationListener(LocationListenerSettings settings)
        {
            this.settings = settings;
            locator = new Geolocator();
        }

        public event EventHandler<Xodium.Services.PositionChangedEventArgs> PositionChanged;

        private void Locator_PositionChanged(Geolocator sender, Windows.Devices.Geolocation.PositionChangedEventArgs args)
        {
            LastKnownPosition = args.Position.ToTrackedPosition();
            PositionChanged?.Invoke(this, new Xodium.Services.PositionChangedEventArgs(LastKnownPosition));
        }

        public Task Start()
        {
            locator.ReportInterval = (uint)settings.MinimumTime.TotalMilliseconds;
            locator.MovementThreshold = settings.MinimumDistance.Meters;
            locator.PositionChanged += Locator_PositionChanged;
            IsListening = true;
            return Task.CompletedTask;
        }

        public Task Stop()
        {
            locator.PositionChanged -= Locator_PositionChanged;
            IsListening = false;
            return Task.CompletedTask;
        }
    }

    public static class GeopositionExtensions
    {
        public static TrackedPosition ToTrackedPosition(this Geoposition value)
        {
            return new TrackedPosition
            {
                Position = value.Coordinate.Point.Position.ToGeoPosition(),
                Heading = value.Coordinate.Heading,
                Speed = value.Coordinate.Speed,
                Time = value.Coordinate.Timestamp
            };
        }

        public static GeoPosition ToGeoPosition(this BasicGeoposition value)
        {
            return new GeoPosition(value.Latitude, value.Longitude, value.Altitude);
        }
    }
}
