using System.ComponentModel.DataAnnotations;

namespace MyParentApi.Application.DTOs.Requests.Profile
{
    public record PasswordRecoveryRequest
    {
        [Required] public string Email { get; set; }
    }
}
