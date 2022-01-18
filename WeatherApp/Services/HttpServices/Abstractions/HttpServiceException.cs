using System;
using System.Net;

namespace Services.HttpServices.Abstractions
{
    public class HttpServiceException : Exception
    {
        public HttpStatusCode StatusCode { get; private set; }
        public HttpServiceException(HttpStatusCode statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
