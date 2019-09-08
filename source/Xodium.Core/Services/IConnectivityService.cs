using System;

namespace Xodium.Services
{
    public enum ConnectionState
    {
        None,
        Global,
        LocalOnly,
        Constrained,
        Unknown
    }

    public class ConnectionStateChangedEventArgs : EventArgs
    {
        public ConnectionStateChangedEventArgs(ConnectionState connectionState)
        {
            ConnectionState = connectionState;
        }

        public ConnectionState ConnectionState { get; }
    }

    public interface IConnectivityService
    {
        ConnectionState ConnectionState { get; }

        event EventHandler<ConnectionStateChangedEventArgs> ConnectionStateChanged;
    }
}
