using Microsoft.Extensions.Logging;
using MyParentApi.Application.DTOs.Requests;
using MyParentApi.Application.DTOs.Requests.Auth;
using MyParentApi.Application.DTOs.Requests.Profile;
using MyParentApi.Application.DTOs.Requests.Users;
using MyParentApi.Application.DTOs.Responses;
using MyParentApi.Application.Interfaces;
using MyParentApi.Application.Managers;
using MyParentApi.DAL.Entities;
using MyParentApi.DAL.Interfaces;
using System.Text.Json;

namespace MyParentApi.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly ILogger<AuthService> logger;

        private readonly ISysLogService sysLogService;
        private readonly ITokenService tokenService;
        private readonly IUserRepository userRepository;

        public AuthService(
            ILogger<AuthService> logger,
            ITokenService tokenService, 
            ISysLogService sysLog,
            IUserRepository userRepository)
        {
            this.logger = logger;
            this.tokenService = tokenService;
            this.sysLogService = sysLog;
            this.userRepository = userRepository;
        }

        private bool CheckPassword(string password, string passwordHash, string salt)
        {
            var chkPassword = WhirlPoolHashService.HashPassword(password, salt);
            return chkPassword.Equals(passwordHash);
        }

        public async Task<AuthResponse> AuthUserAsync(AuthRequest request)
        {
            try
            {
                var user = await userRepository.GetUserAsync(request.Email);
                if (user == null || !CheckPassword(request.Password, user.PasswordHash, user.Salt))
                {
                    return new AuthResponse(SystemErrorCode_InvalidCredentials);
                }

                return new AuthResponse(SystemErrorCode_LoginOk, tokenService.GenerateToken(user));
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw new Exception(ex.ToString());
            }
        }

        public async Task<CreateUserResponse> CreateUserAsync(CreateUserRequest request)
        {
            try
            {
                var user = await userRepository.GetUserAsync(request.Email);
                if (user != null)
                {
                    return new CreateUserResponse();
                }

                byte roleTypeId = RoleTypeUser;
                if (request.RoleType != 0)
                {
                    roleTypeId = request.RoleType;
                }

                // Gerar seed de senha e armazenar junto com a senha cryptografada
                var salt = WhirlPoolHashService.GenerateSalt();
                var hashedPass = WhirlPoolHashService.HashPassword(request.Password, salt);
                var dbUser = new ApiUser()
                {
                    Email = request.Email,
                    PasswordHash = hashedPass,
                    Salt = salt,
                    CreatedAt = DateTime.Now,
                    Name = request.Name,
                    Status = StatusActive,
                    Type = roleTypeId,
                };

                var newUser = await userRepository.CreateUserAsync(dbUser, roleTypeId);
                if (newUser == null)
                {
                    throw new SystemException("Could not create the user!");
                }

                await sysLogService.SaveUserLogAsync(dbUser.Id, "New User Create Operation", JsonSerializer.Serialize(dbUser));
                return new CreateUserResponse(dbUser.Email, dbUser.Name);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw new SystemException(ex.ToString());
            }
        }

        public async Task<GenericResponse> RecoveryPasswordAsync(PasswordRecoveryRequest request)
        {
            try
            {
                var user = await userRepository.GetUserAsync(request.Email);
                if (user == null)
                {
                    return new GenericResponse("Erro", "Usuário de email não cadastrado!");
                }

                var token = WhirlPoolHashService.GenerateSalt();
                if (!await userRepository.CreatePassRecoveryAsync(request.Email, token))
                {
                    throw new SystemException("Could not create the recovery request!");
                }

                await sysLogService.SaveUserLogAsync(user.Id, "Password Recovery Request", $"Email: {request.Email}; Token:{token}");
                return new GenericResponse("", token);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw new SystemException(ex.ToString());
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
    }
}
