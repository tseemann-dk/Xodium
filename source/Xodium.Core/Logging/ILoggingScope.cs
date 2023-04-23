using System;

namespace Xodium.Logging
{
    public interface ILoggingScope : IAsyncDisposable
    {
        ILogger Logger { get; }
    }
}
