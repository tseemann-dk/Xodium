using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Xodium.Net.Http
{
    public class FormUrlEncodedContentBuilder : IContentBuilder
    {
        private readonly IEnumerable<KeyValuePair<string, string>> values;

        public FormUrlEncodedContentBuilder(IEnumerable<KeyValuePair<string, string>> values)
        {
            this.values = values;
        }

        public Task<HttpContent> BuildContent() => Task.FromResult<HttpContent>(new FormUrlEncodedContent(values));
    }
}
