using System.Net;

namespace MyParentApi.Shared.Helpers.Exceptions
{
    public sealed class DatabaseException : HttpExceptionBase
    {
        public DatabaseException(string path, string error)
            : base(path, error)
        {
        }

        public DatabaseException(string path, Exception ex)
            : base(path, ex)
        {
        }
    }
}
