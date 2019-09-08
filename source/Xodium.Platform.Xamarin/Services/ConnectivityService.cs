using System;
using Xamarin.Essentials;
using Xodium.Services;

namespace Xodium.Platform.Xamarin.Services
{
    public class ConnectivityService : IConnectivityService
    {
        public ConnectivityService()
        {
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }

        public ConnectionState ConnectionState => ToConnectionState(Connectivity.NetworkAccess);

        public event EventHandler<ConnectionStateChangedEventArgs> ConnectionStateChanged;

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            OnConnectionStateChanged(ToConnectionState(e.NetworkAccess));
        }

        private ConnectionState ToConnectionState(NetworkAccess access)
        {
            switch (access)
            {
                case NetworkAccess.None:
                    return ConnectionState.None;
                case NetworkAccess.Local:
                    return ConnectionState.LocalOnly;
                case NetworkAccess.ConstrainedInternet:
                    return ConnectionState.Constrained;
                case NetworkAccess.Internet:
                    return ConnectionState.Global;
                default:
                    return ConnectionState.Unknown;
            }
        }

        private void OnConnectionStateChanged(ConnectionState connectionState)
        {
            ConnectionStateChanged?.Invoke(this, new ConnectionStateChangedEventArgs(connectionState));
        }
    }
}
