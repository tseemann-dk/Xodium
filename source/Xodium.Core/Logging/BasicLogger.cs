using System.IO;
using System.Threading.Tasks;

namespace Xodium.Logging
{
    public class BasicLogger : ILogger
    {
        private readonly TextWriter output;

        public BasicLogger(TextWriter output)
        {
            this.output = output ?? throw new System.ArgumentNullException(nameof(output));
        }

        public void Log(string message) => output.WriteLine(message);
        public Task LogAsync(string message) => output.WriteLineAsync(message);
    }
}
