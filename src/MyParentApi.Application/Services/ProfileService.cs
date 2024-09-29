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

        private readonly ISysLogService sysLogService;
        private readonly IUserRepository userRepository;

        public ProfileService(
            ISysLogService sysLogService,
            IUserRepository userRepository,
            AppDbContext context) 
        {
            this.userRepository = userRepository;
            this.sysLogService = sysLogService;
        }

        public async Task<GenericResponse> ChangePasswordAsync(PasswordChangeRequest request)
        {
            var user = await userRepository.GetUserAsync(request.Email);
            if (user == null)
            {
                throw new ProfileException(GetType().Name, StrAuthInvalid);
            }

            var result = WhirlPoolHashService.CheckPassword(request.OldPassword, user.PasswordHash, user.Salt);
            if (result != Microsoft.AspNetCore.Identity.PasswordVerificationResult.Success)
            {
                throw new ProfileException(GetType().Name, StrUserPasswordNotMatch);
            }

            if (!WhirlPoolHashService.IsValidPassword(request.NewPassword))
            {
                throw new ProfileException(GetType().Name, StrPasswordSecurityLow);
            }

            if (!request.NewPassword.Equals(request.NewPasswordConfirm))
            {
                throw new ProfileException(GetType().Name, StrPasswordChangeNotMatch);
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

            throw new ProfileException(GetType().Name, StrPasswordChangeError);
        }

        public async Task<GenericResponse> ChangePasswordByTokenAsync(PasswordChangeRequest request)
        {
            var user = await userRepository.GetUserAsync(request.Email);
            if (user == null)
            {
                throw new ProfileException(GetType().Name, StrAuthInvalid);
            }

            var recoveryRequest = await userRepository.GetRecoveryKeyAsync(request.Email);
            if (recoveryRequest == null)
            {
                throw new ProfileException(GetType().Name, StrRecoveryInvalid);
            }

            if (!recoveryRequest.Equals(request.OldPassword))
            {
                throw new ProfileException(GetType().Name, StrRecoveryInvalidToken);
            }

            if (!WhirlPoolHashService.IsValidPassword(request.NewPassword))
            {
                throw new ProfileException(GetType().Name, StrPasswordSecurityLow);
            }

            if (!request.NewPassword.Equals(request.NewPasswordConfirm))
            {
                throw new ProfileException(GetType().Name, StrPasswordChangeNotMatch);
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

            throw new ProfileException(GetType().Name, StrPasswordChangeError);
        }
    }
}
