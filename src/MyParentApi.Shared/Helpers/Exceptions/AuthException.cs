using System.Net;

namespace MyParentApi.Shared.Helpers.Exceptions
{
    public sealed class AuthException : HttpExceptionBase
    {
        public AuthException(string path, string error) 
            : base(HttpStatusCode.Unauthorized, path, error)
        {
        }

        public AuthException(string path, Exception ex) 
            : base(HttpStatusCode.Unauthorized, path, ex)
        {
        }
    }
}
