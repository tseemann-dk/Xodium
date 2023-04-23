using System;
using System.Threading.Tasks;
using Xodium.Logging;
using Xunit;

namespace Xodium.Core.Tests
{
    public class LoggingTest
    {
        [Fact]
        public async Task CanUseLoggingScope()
        {
            var context = CreateLoggingContext();
            await using var scope = await context.CreateScope();
            await scope.Logger.LogAsync("abc");
        }

        private static ILoggingContext CreateLoggingContext() => new BasicLoggingContext(Console.Out);
    }
}
