namespace MyParentApi.Application.DTOs.Responses
{
    public record AuthResponse
    {
        public AuthResponse(int errorCode, string token)
        {
            ErrorCode = errorCode;
            Token = token;
        }

        public int ErrorCode { get; set; }
        public string Token { get; set; }
    }
}
