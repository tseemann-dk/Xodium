using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Xodium.Serialization;

namespace Xodium.Net.Http
{
    public abstract class RestClientBase
    {
        private readonly HttpClient httpClient;

        protected RestClientBase(HttpClient httpClient, IObjectSerializer serializer)
            : this(httpClient, RestClientOptions.CreateForJsonSerializer(serializer))
        {
        }

        protected RestClientBase(HttpClient httpClient, RestClientOptions options)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            Options = options ?? throw new ArgumentNullException(nameof(options));
        }

        protected HttpClient HttpClient => httpClient;
        protected RestClientOptions Options { get; }

        protected Task<HttpResponseMessage> Delete(
            string path, CancellationToken cancellationToken) =>
            PerformRequest(HttpMethod.Delete, BuildUri(path), cancellationToken);

        protected Task<TResult> Get<TResult>(
            string path, CancellationToken cancellationToken) =>
            PerformRequest<TResult>(HttpMethod.Get, BuildUri(path), cancellationToken);

        protected Task<TResult> Get<TResult>(
            string path, IEnumerable<KeyValuePair<string, string>> arguments, CancellationToken cancellationToken) =>
            PerformRequest<TResult>(HttpMethod.Get, BuildUri(path, arguments), cancellationToken);

        protected Task Put(
            string path, IContentBuilder contentBuilder, CancellationToken cancellationToken) =>
            PerformRequest<object>(HttpMethod.Put, BuildUri(path), contentBuilder, cancellationToken);

        protected Task Put<TBody>(
            string path, TBody body, CancellationToken cancellationToken) =>
            PerformRequest<object>(HttpMethod.Put, BuildUri(path), ToObjectContentBuilder(body), cancellationToken);

        protected Task Put(
            string path, IEnumerable<KeyValuePair<string, string>> arguments, IContentBuilder contentBuilder, CancellationToken cancellationToken) =>
            PerformRequest<object>(HttpMethod.Put, BuildUri(path, arguments), contentBuilder, cancellationToken);

        protected Task<TResult> Put<TResult>(
            string path, IContentBuilder contentBuilder, CancellationToken cancellationToken) =>
            PerformRequest<TResult>(HttpMethod.Put, BuildUri(path), contentBuilder, cancellationToken);

        protected Task<TResult> Put<TBody, TResult>(
            string path, TBody body, CancellationToken cancellationToken) =>
            PerformRequest<TResult>(HttpMethod.Put, BuildUri(path), ToObjectContentBuilder(body), cancellationToken);

        protected Task<TResult> Put<TResult>(
            string path, IEnumerable<KeyValuePair<string, string>> arguments, IContentBuilder contentBuilder, CancellationToken cancellationToken) =>
            PerformRequest<TResult>(HttpMethod.Put, BuildUri(path, arguments), contentBuilder, cancellationToken);

        protected Task<TResult> Put<TBody, TResult>(
            string path, IEnumerable<KeyValuePair<string, string>> arguments, TBody body, CancellationToken cancellationToken) =>
            PerformRequest<TResult>(HttpMethod.Put, BuildUri(path, arguments), ToObjectContentBuilder(body), cancellationToken);

        protected Task Post(
            string path, IContentBuilder contentBuilder, CancellationToken cancellationToken) =>
            PerformRequest<object>(HttpMethod.Post, BuildUri(path), contentBuilder, cancellationToken);

        protected Task Post<TBody>(
            string path, TBody body, CancellationToken cancellationToken) =>
            PerformRequest<object>(HttpMethod.Post, BuildUri(path), ToObjectContentBuilder(body), cancellationToken);

        protected Task Post(
            string path, IEnumerable<KeyValuePair<string, string>> arguments, IContentBuilder contentBuilder, CancellationToken cancellationToken) =>
            PerformRequest<object>(HttpMethod.Post, BuildUri(path, arguments), contentBuilder, cancellationToken);

        protected Task Post<TBody>(
            string path, IEnumerable<KeyValuePair<string, string>> arguments, TBody body, CancellationToken cancellationToken) =>
            PerformRequest<object>(HttpMethod.Post, BuildUri(path, arguments), ToObjectContentBuilder(body), cancellationToken);

        protected Task<TResult> Post<TResult>(
            string path, IContentBuilder contentBuilder, CancellationToken cancellationToken) =>
            PerformRequest<TResult>(HttpMethod.Post, BuildUri(path), contentBuilder, cancellationToken);

        protected Task<TResult> Post<TBody, TResult>(
            string path, TBody body, CancellationToken cancellationToken) =>
            PerformRequest<TResult>(HttpMethod.Post, BuildUri(path), ToObjectContentBuilder(body), cancellationToken);

        protected Task<TResult> Post<TResult>(
            string path, IEnumerable<KeyValuePair<string, string>> arguments, IContentBuilder contentBuilder, CancellationToken cancellationToken) =>
            PerformRequest<TResult>(HttpMethod.Post, BuildUri(path, arguments), contentBuilder, cancellationToken);

        protected Task<TResult> Post<TBody, TResult>(
            string path, IEnumerable<KeyValuePair<string, string>> arguments, TBody body, CancellationToken cancellationToken) =>
            PerformRequest<TResult>(HttpMethod.Post, BuildUri(path, arguments), ToObjectContentBuilder(body), cancellationToken);

        protected IContentBuilder ToObjectContentBuilder<T>(T value, string mediaType = null) => 
            new ObjectContentBuilder<T>(value, Options.Serializer, mediaType ?? Options.DefaultRequestMediaType);

        private async Task<HttpResponseMessage> PerformRequest(
            HttpMethod method, string uri, CancellationToken cancellationToken)
        {
            using (var request = CreateRequest(method, uri))
            {
                return await SendRequest(request, cancellationToken);
            }
        }

        private async Task<TResult> PerformRequest<TResult>(
            HttpMethod method, string uri, CancellationToken cancellationToken)
        {
            using (var request = CreateRequest(method, uri))
            {
                return await PerformRequest<TResult>(request, cancellationToken);
            }
        }

        private async Task<TResult> PerformRequest<TResult>(
            HttpMethod method, string uri, IContentBuilder contentBuilder, CancellationToken cancellationToken)
        {
            using (var content = await contentBuilder.BuildContent())
            using (var request = CreateRequest(method, uri, content))
            {
                return await PerformRequest<TResult>(request, cancellationToken);
            }
        }

        private Task<TResult> PerformRequest<TResult>(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return SendRequestAndReadResponse(request,
                async (response, stream) =>
                {
                    try
                    {
                        return await Options.Serializer.DeserializeAsync<TResult>(stream);
                    }
                    catch (Exception exception)
                    {
                        var data = await StreamToString(stream);
                        throw new RestClientException($"Deserialization error: {exception.Message}\n\nResponse:\n{data}", exception);
                    }
                }, cancellationToken);
        }

        private static async Task<string> StreamToString(Stream stream)
        {
            if (stream.CanSeek)
            {
                stream.Seek(0, SeekOrigin.Begin);
            }
            
            using (var reader = new StreamReader(stream))
            {
                return await reader.ReadToEndAsync();
            }
        }

        private Task<HttpResponseMessage> SendRequest(HttpRequestMessage request, CancellationToken cancellationToken) =>
            httpClient.SendAsync(request, cancellationToken);

        private async Task<TResult> SendRequestAndReadResponse<TResult>(
            HttpRequestMessage request,
            Func<HttpResponseMessage, Stream, Task<TResult>> readResponse,
            CancellationToken cancellationToken)
        {
            if (readResponse is null)
            {
                throw new ArgumentNullException(nameof(readResponse));
            }

            using (var response = await SendRequest(request, cancellationToken))
            {
                if (!response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"{(int)response.StatusCode} {response.ReasonPhrase}\n{result}");
                }

                using (var stream = await response.Content.ReadAsStreamAsync())
                {
                    return await readResponse(response, stream);
                }
            }
        }

        private HttpRequestMessage CreateRequest(
            HttpMethod method, string uri, HttpContent content = null, string acceptMediaType = null)
        {
            var request = new HttpRequestMessage(method, uri);
            acceptMediaType = acceptMediaType ?? Options.DefaultResponseMediaType;
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(acceptMediaType));
            request.Content = content;
            return request;
        }

        private string BuildUri(string path, IEnumerable<KeyValuePair<string, string>> arguments = null)
        {
            return arguments?.Any() ?? false
                ? path + "?" + string.Join("&", arguments.Select(ToQueryParameter))
                : path;
        }

        private string ToQueryParameter(KeyValuePair<string, string> argument) => $"{argument.Key}={argument.Value}";
    }
}

