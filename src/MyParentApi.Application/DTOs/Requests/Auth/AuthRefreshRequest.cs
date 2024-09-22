namespace MyParentApi.Application.DTOs.Requests
{
    public record AuthRefreshRequest
    {
        public AuthRefreshRequest(string token, int time = 0)
        {
            Token = token;
            Time = time;
        }

        public string Token { get; init; }
        public int Time { get; init; }
    }
}
