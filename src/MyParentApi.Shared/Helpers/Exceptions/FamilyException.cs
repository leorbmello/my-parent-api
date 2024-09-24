using System.Net;

namespace MyParentApi.Shared.Helpers.Exceptions
{
    public sealed class FamilyException : HttpExceptionBase
    {
        public FamilyException(string path, string error)
            : base(HttpStatusCode.BadRequest, path, error)
        {
        }

        public FamilyException(string path, Exception ex)
            : base(HttpStatusCode.BadRequest, path, ex)
        {
        }
    }
}
