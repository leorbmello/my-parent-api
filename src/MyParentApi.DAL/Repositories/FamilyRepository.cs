using Microsoft.EntityFrameworkCore;
using MyParentApi.DAL.Entities;
using MyParentApi.DAL.Interfaces;

namespace MyParentApi.DAL.Repositories
{
    public class FamilyRepository : IFamilyRepository
    {
        private readonly AppDbContext context;

        public FamilyRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<ApiFamily> GetFamilyAsync(string name)
        {
            var family = await context.Families
                .Include(x => x.User)
                .Include(x => x.FamilyMembers)
                .FirstOrDefaultAsync(x => x.Name.Equals(name));

            return family;
        }

        public async Task<ApiFamily> GetFamilyByIdAsync(int id)
        {
            var family = await context.Families
                .Include(x => x.User)
                .Include(x => x.FamilyMembers)
                .FirstOrDefaultAsync(x => x.Id == id);

            return family;
        }

        public async Task<ApiFamily> GetFamilyByUserIdAsync(int id)
        {
            var familyMember = await context.FamilyUsers.FirstOrDefaultAsync(x => x.UserId == id);
            if (familyMember == null)
            {
                return null;
            }

            var family = await context.Families
                .Include(x => x.User)
                .Include(x => x.FamilyMembers)
                .FirstOrDefaultAsync(x => x.Id == familyMember.FamilyId);

            return family;
        }

        public async Task<ApiFamily> CreateFamilyAsync(ApiUser user, string name)
        {
            var family = new ApiFamily()
            {
                Name = name,
                UserCreatorId = user.Id,
                Status = StatusActive,
                CreatedAt = DateTime.Now,
            };

            if (await context.CreateAsync(family))
            {
                var familyUser = new ApiFamilyUser()
                {
                    FamilyId = family.Id,
                    UserId = user.Id,
                };

                await context.CreateAsync(familyUser);
                return family;
            }

            return null;
        }

        public async Task<bool> UpdateFamilyAsync(ApiFamily family)
        {
            family.ModifiedAt = DateTime.Now;
            return await context.UpdateAsync(family);
        }

        public async Task<bool> JoinFamilyAsync(int familyId, int userId)
        {
            var familyUser = new ApiFamilyUser()
            {
                FamilyId = familyId,
                UserId = userId,
            };

            return await context.CreateAsync(familyUser);
        }

        public async Task<bool> DeleteMemberAsync(int id)
        {
            var member = await context.FamilyUsers.FirstOrDefaultAsync(x => x.UserId == id);
            if (member == null)
            {
                return false;
            }

            return await context.DeleteAsync(member);
        }

        public async Task<bool> DeleteFamilyAsync(int id)
        {
            var family = await GetFamilyByIdAsync(id);
            if (family == null)
            {
                return false;
            }

            if (family.FamilyMembers != null && family.FamilyMembers.Count > 0)
            {
                await context.DeleteRangeAsync(family.FamilyMembers);
            }

            return await context.DeleteAsync(family);
        }

        public async Task<bool> DeleteFamilyByCreatorIdAsync(int id)
        {
            var family = await GetFamilyByUserIdAsync(id);
            if (family == null)
            {
                return false;
            }

            if (family.UserCreatorId != id)
            {
                return false;
            }

            if (family.FamilyMembers != null && family.FamilyMembers.Count > 0)
            {
                await context.DeleteRangeAsync(family.FamilyMembers);
            }

            return await context.DeleteAsync(family);
        }
    }
}
