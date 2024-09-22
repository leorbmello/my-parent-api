using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyParentApi.Application.DTOs.Requests.Profile;
using MyParentApi.Application.DTOs.Responses;
using MyParentApi.Application.Interfaces;
using System.Text.Json;

namespace MyParentApi.Application.Services
{
    public class ProfileService : IProfileService
    {
        private readonly ILogger<ProfileService> logger;

        private readonly ISysLogService sysLogService;
        private readonly AppDbContext context;

        public ProfileService(
            ILogger<ProfileService> logger,
            ISysLogService sysLogService,
            AppDbContext context) 
        {
            this.logger = logger;
            this.context = context;
            this.sysLogService = sysLogService;
        }

        public async Task<GenericResponse> ChangePasswordAsync(PasswordChangeRequest request)
        {
            try
            {
                var user = await context.Users.FirstOrDefaultAsync(x => x.Email == request.Email);
                if (user == null)
                {
                    return new GenericResponse("Erro", "Usuário inválido!");
                }

                var result = WhirlPoolHashService.CheckPassword(request.OldPassword, user.PasswordHash, user.Salt);
                if (result != Microsoft.AspNetCore.Identity.PasswordVerificationResult.Success)
                {
                    return new GenericResponse("Erro", "Senhas não coincidem!");
                }

                if (!WhirlPoolHashService.IsValidPassword(request.NewPassword))
                {
                    return new GenericResponse("Erro", "A senha não atinge os critérios válidos de segurança!");
                }

                if (!request.NewPassword.Equals(request.NewPasswordConfirm))
                {
                    return new GenericResponse("Erro", "Falha na confirmação de nova senha!");
                }

                var newSalt = WhirlPoolHashService.GenerateSalt();
                var newPass = WhirlPoolHashService.HashPassword(request.NewPassword, newSalt);
                user.PasswordHash = newPass;
                user.Salt = newSalt;
                if (await context.UpdateAsync(user))
                {
                    await sysLogService.SaveUserLogAsync(user.Id, "Password Change", JsonSerializer.Serialize(user));
                    return new GenericResponse("Aviso", "Senha alterada com sucesso!");
                }

                return new GenericResponse("Erro", "Não foi possível alterar a senha, contate um administrador!");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw new SystemException(ex.ToString());
            }
        }

        public async Task<GenericResponse> ChangePasswordByTokenAsync(PasswordChangeRequest request)
        {
            try
            {
                var user = await context.Users.FirstOrDefaultAsync(x => x.Email == request.Email);
                if (user == null)
                {
                    return new GenericResponse("Erro", "Usuário inválido!");
                }

                var recoveryRequest = await context.UserRecovery.FirstOrDefaultAsync(x => x.Email == request.Email);
                if (recoveryRequest == null)
                {
                    return new GenericResponse("Erro", "Modo de recuperação inválido!");
                }

                if (!recoveryRequest.Token.Equals(request.OldPassword))
                {
                    return new GenericResponse("Erro", "Token fornecido para recuperação é inválido!");
                }

                if (!WhirlPoolHashService.IsValidPassword(request.NewPassword))
                {
                    return new GenericResponse("Erro", "A senha não atinge os critérios válidos de segurança!");
                }

                if (!request.NewPassword.Equals(request.NewPasswordConfirm))
                {
                    return new GenericResponse("Erro", "Falha na confirmação de nova senha!");
                }

                var newSalt = WhirlPoolHashService.GenerateSalt();
                var newPass = WhirlPoolHashService.HashPassword(request.NewPassword, newSalt);
                user.PasswordHash = newPass;
                user.Salt = newSalt;
                if (await context.UpdateAsync(user))
                {
                    await context.DeleteAsync(recoveryRequest);
                    await sysLogService.SaveUserLogAsync(user.Id, "Password Change", JsonSerializer.Serialize(user));
                    return new GenericResponse("Aviso", "Senha alterada com sucesso!");
                }

                return new GenericResponse("Erro", "Não foi possível alterar a senha, contate um administrador!");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw new SystemException(ex.ToString());
            }
        }
    }
}
