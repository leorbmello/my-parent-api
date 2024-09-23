using System.Net;

namespace MyParentApi.Shared.Helpers.Exceptions
{
    public sealed class RegistrationException : HttpExceptionBase
    {
        public RegistrationException(string path, string error)
            : base(HttpStatusCode.InternalServerError, path, error)
        {
        }

        public RegistrationException(string path, Exception ex)
            : base(HttpStatusCode.InternalServerError, path, ex)
        {
        }
    }
}
