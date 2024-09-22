using MyParentApi.Application.DTOs.Requests;
using MyParentApi.Application.DTOs.Responses;

namespace MyParentApi.Application.Interfaces
{
    public interface IAuthService
    {
        AuthResponse Authenticate(AuthRequest request);
        Task<CreateUserResponse> CreateUserAsync(CreateUserRequest request);
        void Logout(AuthRefreshRequest request);
        bool CheckPassword(string password, string passwordHash, string salt);
    }
}
