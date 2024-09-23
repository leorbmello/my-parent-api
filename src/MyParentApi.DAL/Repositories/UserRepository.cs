using Microsoft.EntityFrameworkCore;
using MyParentApi.DAL.Entities;
using MyParentApi.DAL.Interfaces;

namespace MyParentApi.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext context;

        public UserRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<ApiUser> GetUserAsync(string email)
        {
            var user = await context.Users
                .Include(x => x.UserInfo)
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(x => x.Email == email);

            return user;
        }

        public async Task<ApiUser> GetUserByIdAsync(int userId)
        {
            var user = await context.Users
                .Include(x => x.UserInfo)
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(x => x.Id == userId);

            return user;
        }

        public async Task<ApiUser> CreateUserAsync(ApiUser newUser, byte roleType)
        {
            if (!await context.CreateAsync(newUser))
            {
                throw new SystemException(StrNewUserCannotSave);
            }

            var dbUserRole = new ApiUserRole()
            {
                RoleId = newUser.Type,
                UserId = newUser.Id,
            };

            if (!await context.CreateAsync(dbUserRole))
            {
                throw new SystemException(string.Format(StrNewUserRoleNotCreated, newUser.Email));
            }

            return newUser;
        }

        public async Task<bool> UpdateUserAsync(ApiUser user)
        {
            if (user == null || user.Id == 0)
            {
                return false;
            }

            return await context.UpdateAsync(user);
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            var user = await GetUserByIdAsync(userId);
            if (user == null) 
            {
                return false;
            }

            user.Status = 3;
            return await context.UpdateAsync(user);
        }

        public async Task<ApiUserInfo> CreateUserInfoAsync(string firstName, string lastName, DateTime birthDate, byte gender)
        {
            var userInfo = new ApiUserInfo()
            {
                FirstName = firstName,
                LastName = lastName,
                Gender = gender,
                BirthDate = birthDate,
                CreatedAt = DateTime.UtcNow,
            };

            if (await context.CreateAsync(userInfo))
            {
                return userInfo;
            }

            return null;
        }

        public async Task<bool> UpdateUserInfoAsync(ApiUserInfo user)
        {
            if (user == null || user.Id == 0)
            {
                return false;
            }

            user.ChangedAt = DateTime.UtcNow;
            return await context.UpdateAsync(user);
        }

        public async Task<string> GetRecoveryKeyAsync(string email)
        {
            var recovery = await context.UserRecovery.FirstOrDefaultAsync(x => x.Email.Equals(email));
            return recovery?.Token ?? null;
        }

        public async Task<bool> CreatePassRecoveryAsync(string email, string passRecoveryKey)
        {
            var token = await GetRecoveryKeyAsync(email);
            if (token != null)
            {
                return true;
            }

            var recovery = new ApiUserRecovery()
            {
                Email = email,
                Token = token
            };

            return await context.CreateAsync(recovery);
        }

        public async Task<bool> DeletePassRecoveryAsync(string passRecoveryKey)
        {
            var recovery = await context.UserRecovery.FirstOrDefaultAsync(x => x.Token.Equals(passRecoveryKey));
            if (recovery == null)
            {
                return false;
            }

            return await context.DeleteAsync(recovery);
        }
    }
}
