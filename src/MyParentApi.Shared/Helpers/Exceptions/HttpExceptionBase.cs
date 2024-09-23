using System.Net;

namespace MyParentApi.Shared.Helpers.Exceptions
{
    public class HttpExceptionBase : Exception
    {
        public HttpExceptionBase(HttpStatusCode httpStatusCode, string path, string error)
        {
            StatusCode = (int)httpStatusCode;
            Path = path;
            Error = error;
            Timestamp = DateTimeOffset.UtcNow;
        }

        public HttpExceptionBase(HttpStatusCode httpStatusCode, string path, Exception ex)
            : base(ex.Message, ex)
        {
            StatusCode = (int)httpStatusCode;
            Path = path;
            Error = ex.Message;
            Timestamp = DateTimeOffset.UtcNow;
        }

        public int StatusCode { get; }
        public string Path { get; }
        public string Error { get; }
        public DateTimeOffset Timestamp { get; }
    }
}
