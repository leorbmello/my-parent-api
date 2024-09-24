using MyParentApi.DAL.Entities;

namespace MyParentApi.DAL.Interfaces
{
    public interface IFamilyRepository
    {
        Task<ApiFamily> GetFamilyAsync(string name);
        Task<ApiFamily> GetFamilyByUserIdAsync(int id);
        Task<ApiFamily> GetFamilyByIdAsync(int id);
        Task<ApiFamily> CreateFamilyAsync(ApiUser user, string name);
        Task<bool> UpdateFamilyAsync(ApiFamily family);
        Task<bool> JoinFamilyAsync(int familyId, int userId);
        Task<bool> DeleteMemberAsync(int id);
        Task<bool> DeleteFamilyAsync(int userId, int id);
        Task<bool> DeleteFamilyByCreatorIdAsync(int id);
    }
}
