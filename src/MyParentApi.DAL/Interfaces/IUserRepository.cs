using MyParentApi.DAL.Entities;

namespace MyParentApi.DAL.Interfaces
{
    public interface IUserRepository
    {
        // user main account part
        Task<ApiUser> GetUserAsync(string email);
        Task<ApiUser> GetUserByIdAsync(int userId);
        Task<ApiUser> CreateUserAsync(ApiUser newUser, byte roleType);
        Task<bool> UpdateUserAsync(ApiUser user);
        Task<bool> DeleteUserAsync(int userId);

        // user information part
        Task<ApiUserInfo> CreateUserInfoAsync(string firstName, string lastName, DateTime birthDate, byte gender);
        Task<bool> UpdateUserInfoAsync(ApiUserInfo user);

        // recovery
        Task<string> GetRecoveryKeyAsync(string email);
        Task<bool> CreatePassRecoveryAsync(string email, string passRecoveryKey);
        Task<bool> DeletePassRecoveryAsync(string passRecoveryKey);
    }
}
