namespace MyParentApi.Shared.Helpers.Exceptions
{
    public sealed class RegistrationException : HttpExceptionBase
    {
        public RegistrationException(string path, string error)
            : base(path, error)
        {
        }

        public RegistrationException(string path, Exception ex)
            : base(path, ex)
        {
        }
    }
}
