using Microsoft.Extensions.Logging;
using MyParentApi.Application.DTOs.Requests;
using MyParentApi.Application.DTOs.Responses;
using MyParentApi.Application.Interfaces;
using MyParentApi.Application.Managers;

namespace MyParentApi.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly ITokenService tokenService;
        private readonly AppDbContext context;
        private readonly ILogger<AuthService> logger;
        public AuthService(
            ILogger<AuthService> logger,
            ITokenService tokenService, 
            AppDbContext context)
        {
            this.logger = logger;
            this.tokenService = tokenService;
            this.context = context;
        }

        public AuthResponse Authenticate(AuthRequest request)
        {
            try
            {
                var user = context.Users.FirstOrDefault(x => x.Email.Equals(request.Email));
                if (user == null || !ValidatePassword(request.Password, user.PasswordHash))
                {
                    return new AuthResponse(SystemErrorCode_InvalidCredentials, null);
                }

                return new AuthResponse(SystemErrorCode_LoginOk, tokenService.GenerateToken(user.Email));
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                return new AuthResponse(SystemErrorCode_InvalidCredentials, null);
            }
        }

        public void Logout(AuthRefreshRequest request)
        {
            if (string.IsNullOrEmpty(request.Token))
            {
                return;
            }

            TokenBlackListMgr.AddToBlackList(request.Token);
        }

        public bool ValidatePassword(string password, string passwordHash)
        {
            return true;
        }
    }
}
