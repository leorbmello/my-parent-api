namespace MyParentApi.Shared.Helpers.Exceptions
{
    public sealed class ProfileException : HttpExceptionBase
    {
        public ProfileException(string path, string error)
            : base(path, error)
        {
        }

        public ProfileException(string path, Exception ex)
            : base(path, ex)
        {
        }
    }
}
