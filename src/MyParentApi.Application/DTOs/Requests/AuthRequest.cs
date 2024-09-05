namespace MyParentApi.Application.DTOs.Requests
{
    public record AuthRequest
    {
        public AuthRequest(string email, string password)
        {
            Email = email;
            Password = password;
        }

        public string Email { get; init; }
        public string Password { get; init; }
    }
}
