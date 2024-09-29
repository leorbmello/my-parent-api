namespace MyParentApi.Shared.Helpers.Exceptions
{
    public class HttpExceptionBase : Exception
    {
        public HttpExceptionBase(string path, string error)
        {
            Path = path;
            Error = error;
            Timestamp = DateTimeOffset.UtcNow;
        }

        public HttpExceptionBase(string path, Exception ex)
            : base(ex.Message, ex)
        {
            Path = path;
            Error = ex.Message;
            Timestamp = DateTimeOffset.UtcNow;
        }

        public string Path { get; }
        public string Error { get; }
        public DateTimeOffset Timestamp { get; }
    }
}
