using MyParentApi.Application.DTOs.Requests;
using MyParentApi.Application.DTOs.Responses;
using MyParentApi.Application.Interfaces;

namespace MyParentApi.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly ITokenService tokenService;
        private readonly AppDbContext context;

        public AuthService(ITokenService tokenService, AppDbContext context)
        {
            this.tokenService = tokenService;
            this.context = context;
        }

        public AuthResponse Authenticate(AuthRequest request)
        {
            var user = context.Users.FirstOrDefault(x => x.Email.Equals(request.Email));
            if (user == null || !ValidatePassword(request.Password, user.PasswordHash))
            {
                return new AuthResponse(SystemErrorCode_InvalidCredentials, null);
            }

            return new AuthResponse(SystemErrorCode_LoginOk, tokenService.GenerateToken(user.Email));
        }

        public bool ValidatePassword(string password, string passwordHash)
        {
            return true;
        }
    }
}
