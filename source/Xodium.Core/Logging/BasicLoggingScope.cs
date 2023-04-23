using System.IO;
using System.Threading.Tasks;

namespace Xodium.Logging
{
    public class BasicLoggingScope : ILoggingScope
    {
        private readonly TextWriter output;

        public BasicLoggingScope(TextWriter output)
        {
            this.output = output ?? throw new System.ArgumentNullException(nameof(output));
            Logger = new BasicLogger(output);
        }

        public ILogger Logger { get; }

        public async ValueTask DisposeAsync()
        {
            output.WriteLine("Flushing...");
            await output.FlushAsync();
            output.Dispose();
        }
    }
}
