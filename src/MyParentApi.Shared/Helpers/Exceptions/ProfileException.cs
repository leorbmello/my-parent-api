using System.Net;

namespace MyParentApi.Shared.Helpers.Exceptions
{
    public sealed class ProfileException : HttpExceptionBase
    {
        public ProfileException(string path, string error)
            : base(HttpStatusCode.UnprocessableContent, path, error)
        {
        }

        public ProfileException(string path, Exception ex)
            : base(HttpStatusCode.UnprocessableContent, path, ex)
        {
        }
    }
}
