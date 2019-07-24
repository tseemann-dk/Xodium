using System;

namespace Xodium.Mvvm
{
    public class NavigationException : Exception
    {
        public NavigationException(string message, Exception innerException = null)
            : base(message, innerException)
        {
        }
    }
}
