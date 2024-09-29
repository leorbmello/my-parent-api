namespace MyParentApi.Shared.Helpers.Exceptions
{
    public sealed class AuthException : HttpExceptionBase
    {
        public AuthException(string path, string error) 
            : base(path, error)
        {
        }

        public AuthException(string path, Exception ex) 
            : base(path, ex)
        {
        }
    }
}
