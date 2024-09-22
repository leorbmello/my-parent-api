using System.ComponentModel.DataAnnotations;

namespace MyParentApi.Application.DTOs.Requests
{
    public record CreateUserRequest
    {
        [Required] public string Email { get; init; }
        [Required] public string Name { get; init; }
        [Required] public string Password { get; init; }
        public byte RoleType { get; init; }
    }
}
