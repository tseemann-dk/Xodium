using Xodium.Data.Transport.Json.Microsoft;
using Xunit;

namespace Xodium.Data.Transport.Tests
{
    public class JsonObjectStreamTest
    {
        [Fact]
        public void CanSkipUntilObjectStart()
        {
            using var jsonStream = SampleData.GetJsonAsStream();
            using var objectStream = new JsonObjectStream(jsonStream);

            var data = new byte[1];
            
            Assert.True(objectStream.SkipUntilObjectStart());
            Assert.Equal(1, objectStream.Read(data));
            Assert.Equal((byte)'{', data[0]);
        }
    }
}
