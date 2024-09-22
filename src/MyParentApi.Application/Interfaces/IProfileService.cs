using MyParentApi.Application.DTOs.Requests.Profile;
using MyParentApi.Application.DTOs.Responses;

namespace MyParentApi.Application.Interfaces
{
    public interface IProfileService
    {
        Task<GenericResponse> ChangePasswordAsync(PasswordChangeRequest request);
        Task<GenericResponse> ChangePasswordByTokenAsync(PasswordChangeRequest request);
    }
}
