using System;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Gms.Location;
using Android.Locations;
using Android.OS;
using Xodium.Geography;
using Xodium.Services;

namespace Xodium.Platform.Android.Services
{
    public class LocationService : ILocationService
    {
        private readonly Activity activity;

        public LocationService(Activity activity)
        {
            this.activity = activity ?? throw new ArgumentNullException(nameof(activity));
        }

        public async Task<TrackedPosition> GetCurrentPosition(TimeSpan maximumAge, TimeSpan timeout, CancellationToken cancellationToken = default(CancellationToken))
        {
            var client = LocationServices.GetFusedLocationProviderClient(activity);

            if (maximumAge > TimeSpan.MinValue)
            {
                if ((await client.GetLocationAvailabilityAsync())?.IsLocationAvailable ?? false)
                {
                    var location = await client.GetLastLocationAsync();

                    if (location != null)
                    {
                        var elapsedTime = TimeSpan.FromMilliseconds((SystemClock.ElapsedRealtimeNanos() - location.ElapsedRealtimeNanos) / 1000000);

                        if (elapsedTime <= maximumAge)
                            return location.ToTrackedPosition();
                    }
                }
            }

            return await RetrieveCurrentPosition(timeout, cancellationToken);
        }

        private async Task<TrackedPosition> RetrieveCurrentPosition(TimeSpan timeout, CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<TrackedPosition>();
            var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            var listener = GetListener(LocationListenerSettings.Default);

            cts.CancelAfter(timeout);
            cts.Token.Register(() => tcs.TrySetCanceled());

            listener.PositionChanged += (s, e) =>
            {
                if (e.Position != null && !cts.IsCancellationRequested)
                {
                    tcs.SetResult(e.Position);
                }
            };

            try
            {
                await listener.Start();
                var position = await tcs.Task;
                await listener.Stop();
                return position;
            }
            catch (TaskCanceledException)
            {
                return null;
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine(exception.Message);
                throw;
            }
        }

        public Xodium.Services.ILocationListener GetListener(LocationListenerSettings settings)
        {
            return new LocationListener(activity, settings ?? LocationListenerSettings.Default);
        }
    }

    internal class LocationListener : LocationCallback, Xodium.Services.ILocationListener
    {
        private readonly Activity activity;
        private readonly LocationListenerSettings settings;
        private readonly FusedLocationProviderClient client;

        public LocationListener(Activity activity, LocationListenerSettings settings)
        {
            this.activity = activity ?? throw new ArgumentNullException(nameof(activity));
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
            client = LocationServices.GetFusedLocationProviderClient(activity);
        }

        public bool IsListening { get; private set; }
        public TrackedPosition LastKnownPosition { get; private set; }

        public event EventHandler<PositionChangedEventArgs> PositionChanged;

        public async Task Start()
        {
            var request = new LocationRequest()
                .SetPriority(LocationRequest.PriorityHighAccuracy)
                .SetInterval((long)settings.MinimumTime.TotalMilliseconds)
                .SetSmallestDisplacement((long)settings.MinimumDistance.Meters);

            await client.RequestLocationUpdatesAsync(request, this, activity.MainLooper);
            IsListening = true;
        }

        public Task Stop()
        {
            // TODO: await client.RemoveLocationUpdatesAsync(this)
            client.RemoveLocationUpdates(this);
            IsListening = false;
            return Task.CompletedTask;
        }

        public override void OnLocationResult(LocationResult result)
        {
            LastKnownPosition = result.LastLocation.ToTrackedPosition();
            PositionChanged?.Invoke(this, new PositionChangedEventArgs(LastKnownPosition));
            base.OnLocationResult(result);
        }
    }

    public static class LocationExtensions
    {
        public static TrackedPosition ToTrackedPosition(this Location location) => 
            new TrackedPosition
            {
                Position = new GeoPosition(location.Latitude, location.Longitude, location.Altitude),
                Heading = location.Bearing,
                Speed = location.Speed,
                Time = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(location.Time)
            };
    }
}
