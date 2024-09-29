namespace MyParentApi.Shared.Helpers.Exceptions
{
    public sealed class FamilyException : HttpExceptionBase
    {
        public FamilyException(string path, string error)
            : base(path, error)
        {
        }

        public FamilyException(string path, Exception ex)
            : base(path, ex)
        {
        }
    }
}
