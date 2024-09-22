using System.ComponentModel.DataAnnotations;

namespace MyParentApi.Application.DTOs.Requests.Auth
{
    public record AuthRequest
    {
        public AuthRequest(string email, string password)
        {
            Email = email;
            Password = password;
        }

        [Required] public string Email { get; init; }
        [Required] public string Password { get; init; }
    }
}
