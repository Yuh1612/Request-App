using System.Net;

namespace API.Exceptions
{
    public class HttpResponseException : Exception
    {
        public HttpResponseException(int statusCode, object? value = null) => (StatusCode, Value) = (statusCode, value);

        public HttpResponseException(HttpStatusCode statusCode, object? value = null) => (StatusCode, Value) = ((int)statusCode, value);

        public int StatusCode { get; set; } = 500;
        public object? Value { get; set; }
    }
}