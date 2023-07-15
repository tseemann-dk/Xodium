using System;

namespace Xodium.Net.Http
{
    public class RestClientException : Exception
    {
        public RestClientException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
}
