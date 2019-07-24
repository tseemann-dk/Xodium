using System;

namespace Xodium.Mvvm
{
    public enum NavigationDirection { Forward, Backward }

    public class NavigationEventArgs : EventArgs
    {
        public NavigationEventArgs(NavigationDirection direction)
        {
            Direction = direction;
        }

        public NavigationDirection Direction { get; }
    }

    public delegate void NavigationEventHandler(object sender, NavigationEventArgs args);
}
