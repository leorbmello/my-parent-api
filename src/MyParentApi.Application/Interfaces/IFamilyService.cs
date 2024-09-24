using MyParentApi.Application.DTOs.Requests.Family;
using MyParentApi.Application.DTOs.Responses;
using MyParentApi.DAL.Entities;

namespace MyParentApi.Application.Interfaces
{
    public interface IFamilyService
    {
        Task<ApiFamily> GetFamilyByIdAsync(int familyId);
        Task<ApiFamily> CreateNewFamilyAsync(FamilyCreateRequest request);
        Task<ApiFamily> AcceptFamilyInviteAsync(ApiUser user);
        Task<GenericResponse> DeleteFamilyMemberAsync(ApiUser user, int targetId);
        Task<bool> DeleteFamilyAsync(FamilyDeleteRequest request);
        Task<bool> DeleteFamilyByCreatorIdAsync(int userId);
    }
}
