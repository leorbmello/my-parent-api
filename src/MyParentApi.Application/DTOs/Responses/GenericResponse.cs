namespace MyParentApi.Application.DTOs.Responses
{
    public record GenericResponse
    {
        public GenericResponse(string code, string message)
        {
            Code = code;
            Message = message;
        }

        public string Code { get; init; }
        public string Message { get; init; }
    }
}
