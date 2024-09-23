using System.Net;

namespace MyParentApi.Shared.Helpers.Exceptions
{
    public sealed class DatabaseException : HttpExceptionBase
    {
        public DatabaseException(string path, string error)
            : base(HttpStatusCode.InternalServerError, path, error)
        {
        }

        public DatabaseException(string path, Exception ex)
            : base(HttpStatusCode.InternalServerError, path, ex)
        {
        }
    }
}
