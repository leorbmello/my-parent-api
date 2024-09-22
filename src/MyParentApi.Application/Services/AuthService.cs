using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyParentApi.Application.DTOs.Requests;
using MyParentApi.Application.DTOs.Responses;
using MyParentApi.Application.Interfaces;
using MyParentApi.Application.Managers;
using MyParentApi.DAL.Entities;
using System.Text.Json;

namespace MyParentApi.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly ILogger<AuthService> logger;

        private readonly ISysLogService sysLogService;
        private readonly ITokenService tokenService;
        private readonly AppDbContext context;

        public AuthService(
            ILogger<AuthService> logger,
            ITokenService tokenService, 
            ISysLogService sysLog,
            AppDbContext context)
        {
            this.logger = logger;
            this.tokenService = tokenService;
            this.context = context;
            this.sysLogService = sysLog;
        }

        public bool CheckPassword(string password, string passwordHash, string salt)
        {
            var chkPassword = WhirlPoolHashService.HashPassword(password, salt);
            return chkPassword.Equals(passwordHash);
        }

        public AuthResponse Authenticate(AuthRequest request)
        {
            try
            {
                var user = context.Users.FirstOrDefault(x => x.Email.Equals(request.Email));
                if (user == null || !CheckPassword(request.Password, user.PasswordHash, user.Salt))
                {
                    return new AuthResponse(SystemErrorCode_InvalidCredentials);
                }

                return new AuthResponse(SystemErrorCode_LoginOk, tokenService.GenerateToken(user.Email));
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                return new AuthResponse(SystemErrorCode_InvalidCredentials);
            }
        }

        public async Task<CreateUserResponse> CreateUserAsync(CreateUserRequest request)
        {
            var user = await context.Users.FirstOrDefaultAsync(x=>x.Email.Equals(request.Email));
            if (user != null)
            {
                return new CreateUserResponse();
            }

            // Gerar seed de senha e armazenar junto com a senha cryptografada
            var salt = WhirlPoolHashService.GenerateSalt();
            var hashedPass = WhirlPoolHashService.HashPassword(request.Password, salt);
            var dbUser = new ApiUser()
            {
                Email = request.Email,
                PasswordHash = hashedPass,
                Salt = salt,
                DataCriacao = DateTime.Now,
                DataAlteracao = DateTime.Now,
                DataNascimento = DateTime.Now,
                Nome = request.Name,
                Status = StatusActive,
                Tipo = RoleTypeUser,
            };

            // Tu é burro e fez merda, falhou aqui.
            if (!await context.CreateAsync(dbUser))
            {
                return new CreateUserResponse();
            }

            await sysLogService.SaveUserLogAsync(dbUser.Id, "New User Create Operation", JsonSerializer.Serialize(dbUser));
            return new CreateUserResponse(dbUser.Email, dbUser.Nome, dbUser.PasswordHash, dbUser.Salt);
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
