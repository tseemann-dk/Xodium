using System;

namespace Xodium.Net.Http
{
    public class WebApiException : Exception
    {
        public WebApiException(int statusCode, string content)
            : base($"Web API error {statusCode}: {content}")
        {
            StatusCode = statusCode;
            Content = content;
        }

        public int StatusCode { get; }
        public string Content { get; set; }
    }
}
