using System.ComponentModel.DataAnnotations;

namespace MyParentApi.Application.DTOs.Requests
{
    public record PasswordChangeRequest
    {
        [Required] public string Email { get; init; }
        [Required] public string OldPassword { get; init; }
        [Required] public string NewPassword { get; init; }
        [Required] public string NewPasswordConfirm { get; init; }
    }
}
