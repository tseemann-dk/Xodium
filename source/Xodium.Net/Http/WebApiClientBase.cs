using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xodium.Serialization;

namespace Xodium.Net.Http
{
    public abstract class WebApiClientBase
    {
        private readonly Uri baseUri;
        private readonly HttpClient httpClient;
        private readonly IObjectSerializer serializer;

        protected WebApiClientBase(Uri baseUri, IObjectSerializer serializer, HttpMessageHandler handler = null)
            : this(baseUri, handler == null ? new HttpClient() : new HttpClient(handler, false))
        {
            this.serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        }

        protected WebApiClientBase(Uri baseUri, HttpClient httpClient)
        {
            this.baseUri = baseUri ?? throw new ArgumentNullException(nameof(baseUri));
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        protected HttpClient HttpClient => httpClient;
        protected MediaTypeWithQualityHeaderValue JsonMediaType { get; } = new MediaTypeWithQualityHeaderValue("application/json");

        protected Task<TResult> Get<TResult>(
            string path, IEnumerable<KeyValuePair<string, string>> arguments, CancellationToken cancellationToken)
            => PerformRequest<TResult>(HttpMethod.Get, BuildUri(path, arguments), cancellationToken);

        protected Task<TResult> Put<TBody, TResult>(
            string path, IEnumerable<KeyValuePair<string, string>> arguments, TBody body, CancellationToken cancellationToken)
            => PerformRequest<TBody, TResult>(HttpMethod.Put, BuildUri(path, arguments), body, cancellationToken);

        protected Task Put<TBody>(
            string path, IEnumerable<KeyValuePair<string, string>> arguments, TBody body, CancellationToken cancellationToken)
            => PerformRequest<TBody, object>(HttpMethod.Put, BuildUri(path, arguments), body, cancellationToken);

        protected Task Post<TBody>(
            string path, IEnumerable<KeyValuePair<string, string>> arguments,
            TBody body, CancellationToken cancellationToken)
            => PerformRequest<TBody, object>(HttpMethod.Post, BuildUri(path, arguments), body, cancellationToken);

        protected Task<TResult> Post<TBody, TResult>(
            string path, IEnumerable<KeyValuePair<string, string>> arguments, TBody body, CancellationToken cancellationToken)
            => PerformRequest<TBody, TResult>(HttpMethod.Post, BuildUri(path, arguments), body, cancellationToken);

        protected async Task<TResult> PerformRequest<TBody, TResult>(HttpMethod method, Uri uri, TBody body, CancellationToken cancellationToken)
        {
            using (var stream = new MemoryStream())
            {
                await serializer.Serialize(body, stream);
                stream.Seek(0, SeekOrigin.Begin);
                return await PerformRequest<TResult>(method, uri, cancellationToken, stream);
            }
        }

        protected async Task<TResult> PerformRequest<TResult>(HttpMethod method, Uri uri, CancellationToken cancellationToken, Stream content = null)
        {
            using (var request = CreateRequest(method, uri, content))
            {
                return await PerformRequest<TResult>(request, cancellationToken);
            }
        }

        private async Task<TResult> PerformRequest<TResult>(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            await PrepareRequest(request, cancellationToken);

            if (typeof(TResult) == typeof(object)) return default(TResult);

            using (var response = await httpClient.SendAsync(request, cancellationToken))
            using (var stream = await response.Content.ReadAsStreamAsync())
            {
                if (!response.IsSuccessStatusCode)
                {
                    using (var reader = new StreamReader(stream))
                    {
                        throw new WebApiException((int)response.StatusCode, reader.ReadToEnd());
                    }
                }

                return await serializer.Deserialize<TResult>(stream);
            }
        }

        protected static AuthenticationHeaderValue GetBasicAuthenticationHeader(string username, string password)
        {
            var bytes = Encoding.ASCII.GetBytes($"{username}:{password}");
            return new AuthenticationHeaderValue("Basic", Convert.ToBase64String(bytes));
        }

        protected static AuthenticationHeaderValue GetBearerAuthenticationHeader(string token)
        {
            return new AuthenticationHeaderValue("Bearer", token);
        }

        protected virtual Task PrepareRequest(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        protected virtual HttpRequestMessage CreateRequest(HttpMethod method, Uri uri, Stream content)
        {
            var request = new HttpRequestMessage(method, uri);

            request.Headers.Accept.Add(JsonMediaType);

            if (content != null)
            {
                request.Content = new StreamContent(content);
                request.Content.Headers.ContentType = JsonMediaType;
            }

            return request;
        }

        protected virtual Uri BuildUri(string path, IEnumerable<KeyValuePair<string, string>> arguments = null)
        {
            var builder = new UriBuilder(new Uri(baseUri, path))
            {
                Query = arguments == null ? null : string.Join("&", arguments.Select(ToQueryParameter))
            };

            return builder.Uri;
        }

        private string ToQueryParameter(KeyValuePair<string, string> argument) => $"{argument.Key}={argument.Value}";
    }
}
