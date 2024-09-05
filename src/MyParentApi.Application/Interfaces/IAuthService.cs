using MyParentApi.Application.DTOs.Requests;
using MyParentApi.Application.DTOs.Responses;

namespace MyParentApi.Application.Interfaces
{
    public interface IAuthService
    {
        AuthResponse Authenticate(AuthRequest request);
        bool ValidatePassword(string password, string passwordHash);
    }
}
