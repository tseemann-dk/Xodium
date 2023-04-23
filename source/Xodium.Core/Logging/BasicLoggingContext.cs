using System;
using System.IO;
using System.Threading.Tasks;

namespace Xodium.Logging
{
    public class BasicLoggingContext : ILoggingContext
    {
        private readonly TextWriter output;

        public BasicLoggingContext(TextWriter output)
        {
            this.output = output ?? throw new ArgumentNullException(nameof(output));
        }

        public async Task<ILoggingScope> CreateScope() => await Task.FromResult(new BasicLoggingScope(output));
    }
}
