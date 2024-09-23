using Microsoft.Extensions.Logging;
using MyParentApi.Application.DTOs.Requests.Profile;
using MyParentApi.Application.DTOs.Responses;
using MyParentApi.Application.Interfaces;
using MyParentApi.DAL.Interfaces;
using System.Text.Json;

namespace MyParentApi.Application.Services
{
    public class ProfileService : IProfileService
    {
        private readonly ILogger<ProfileService> logger;

        private readonly ISysLogService sysLogService;
        private readonly IUserRepository userRepository;

        public ProfileService(
            ILogger<ProfileService> logger,
            ISysLogService sysLogService,
            IUserRepository userRepository,
            AppDbContext context) 
        {
            this.logger = logger;
            this.userRepository = userRepository;
            this.sysLogService = sysLogService;
        }

        public async Task<GenericResponse> ChangePasswordAsync(PasswordChangeRequest request)
        {
            try
            {
                var user = await userRepository.GetUserAsync(request.Email);
                if (user == null)
                {
                    return new GenericResponse(StrTitleError, StrAuthInvalid);
                }

                var result = WhirlPoolHashService.CheckPassword(request.OldPassword, user.PasswordHash, user.Salt);
                if (result != Microsoft.AspNetCore.Identity.PasswordVerificationResult.Success)
                {
                    return new GenericResponse(StrTitleError, StrUserPasswordNotMatch);
                }

                if (!WhirlPoolHashService.IsValidPassword(request.NewPassword))
                {
                    return new GenericResponse(StrTitleError, StrPasswordSecurityLow);
                }

                if (!request.NewPassword.Equals(request.NewPasswordConfirm))
                {
                    return new GenericResponse(StrTitleError, StrPasswordChangeNotMatch);
                }

                var newSalt = WhirlPoolHashService.GenerateSalt();
                var newPass = WhirlPoolHashService.HashPassword(request.NewPassword, newSalt);
                user.PasswordHash = newPass;
                user.Salt = newSalt;
                if (await userRepository.UpdateUserAsync(user))
                {
                    await sysLogService.SaveUserLogAsync(user.Id, StrOperationPasswordChange, JsonSerializer.Serialize(user));
                    return new GenericResponse(StrTitleAdvertise, StrPasswordChangeSuccess);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw new ProfileException(GetType().Name, ex);
            }

            return new GenericResponse(StrTitleError, StrPasswordChangeError);
        }

        public async Task<GenericResponse> ChangePasswordByTokenAsync(PasswordChangeRequest request)
        {
            try
            {
                var user = await userRepository.GetUserAsync(request.Email);
                if (user == null)
                {
                    return new GenericResponse(StrTitleError, StrAuthInvalid);
                }

                var recoveryRequest = await userRepository.GetRecoveryKeyAsync(request.Email);
                if (recoveryRequest == null)
                {
                    return new GenericResponse(StrTitleAdvertise, StrRecoveryInvalid);
                }

                if (!recoveryRequest.Equals(request.OldPassword))
                {
                    return new GenericResponse(StrTitleAdvertise, StrRecoveryInvalidToken);
                }

                if (!WhirlPoolHashService.IsValidPassword(request.NewPassword))
                {
                    return new GenericResponse(StrTitleError, StrPasswordSecurityLow);
                }

                if (!request.NewPassword.Equals(request.NewPasswordConfirm))
                {
                    return new GenericResponse(StrTitleError, StrPasswordChangeNotMatch);
                }

                var newSalt = WhirlPoolHashService.GenerateSalt();
                var newPass = WhirlPoolHashService.HashPassword(request.NewPassword, newSalt);
                user.PasswordHash = newPass;
                user.Salt = newSalt;
                if (await userRepository.UpdateUserAsync(user))
                {
                    await userRepository.DeletePassRecoveryAsync(recoveryRequest);
                    await sysLogService.SaveUserLogAsync(user.Id, StrOperationPasswordChange, JsonSerializer.Serialize(user));
                    return new GenericResponse(StrTitleAdvertise, StrPasswordChangeSuccess);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw new ProfileException(GetType().Name, ex);
            }

            return new GenericResponse(StrTitleError, StrPasswordChangeError);
        }
    }
}
