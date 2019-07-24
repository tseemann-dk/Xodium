using System;
using System.Threading.Tasks;
using Xodium.Geography;

namespace Xodium.Services
{
    public class PositionChangedEventArgs
    {
        public PositionChangedEventArgs(TrackedPosition position)
        {
            Position = position;
        }

        public TrackedPosition Position { get; }
    }

    public class LocationListenerSettings
    {
        public LocationListenerSettings(GeoDistance minimumDistance, TimeSpan minimumTime)
        {
            MinimumDistance = minimumDistance;
            MinimumTime = minimumTime;
        }

        public static LocationListenerSettings Default =
            new LocationListenerSettings(GeoDistance.FromMeters(10), TimeSpan.FromSeconds(1));

        public GeoDistance MinimumDistance { get; set; }
        public TimeSpan MinimumTime { get; set; }
    }

    public interface ILocationListener
    {
        bool IsListening { get; }
        TrackedPosition LastKnownPosition { get; }

        Task Start();
        Task Stop();

        event EventHandler<PositionChangedEventArgs> PositionChanged;
    }
}
