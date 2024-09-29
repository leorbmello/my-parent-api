using MyParentApi.Application.DTOs.Requests.Family;
using MyParentApi.Application.DTOs.Responses;
using MyParentApi.Application.Interfaces;
using MyParentApi.DAL.Entities;
using MyParentApi.DAL.Interfaces;

namespace MyParentApi.Application.Services
{
    public class FamilyService : IFamilyService
    {
        private readonly IFamilyRepository familyRepository;
        private readonly IUserRepository userRepository;
        private readonly INotificationRepository notificationRepository;

        public FamilyService(
            IFamilyRepository familyRepository,
            IUserRepository userRepository,
            INotificationRepository notificationRepository)
        {
            this.familyRepository = familyRepository;
            this.userRepository = userRepository;
            this.notificationRepository = notificationRepository;
        }

        public async Task<ApiFamily> GetFamilyByIdAsync(int familyId)
        {
            return await familyRepository.GetFamilyByIdAsync(familyId);
        }

        public async Task<ApiFamily> CreateNewFamilyAsync(FamilyCreateRequest request)
        {
            var user = await userRepository.GetUserAsync(request.Email);
            if (user == null)
            {
                throw new FamilyException(GetType().Name, StrUserNotFound);
            }

            var family = await familyRepository.GetFamilyByUserIdAsync(user.Id);
            if (family == null)
            {
                return await familyRepository.CreateFamilyAsync(user, request.Name);
            }

            throw new FamilyException(GetType().Name, string.Format(StrUserHasFamily, user.Name));
        }

        public async Task<ApiFamily> AcceptFamilyInviteAsync(ApiUser user)
        {
            var notes = await notificationRepository.GetNotesByTypeAsync(user.Id, NoteTypeInvite);
            if (notes.Count < 1)
            {
                throw new FamilyException(GetType().Name, StrUserHasNoInvite);
            }

            var note = notes.FirstOrDefault();
            if (note != null)
            {
                if (!await familyRepository.JoinFamilyAsync(note.FamilyId, user.Id))
                {
                    throw new FamilyException(GetType().Name, StrUserCannotJoinFamily);
                }
            }

            var family = await familyRepository.GetFamilyByUserIdAsync(user.Id);
            if (family == null)
            {
                throw new FamilyException(GetType().Name, StrFamilyNotFound);
            }

            return family;
        }

        public async Task<GenericResponse> DeleteFamilyMemberAsync(ApiUser user, int targetId)
        {
            var family = await familyRepository.GetFamilyByUserIdAsync(user.Id);
            if (family == null)
            {
                throw new FamilyException(GetType().Name, StrUserCannotJoinFamily);
            }

            if (family.UserCreatorId != user.Id)
            {
                throw new FamilyException(GetType().Name, StrUserNotFamilyOwner);
            }

            if (family.UserCreatorId == targetId)
            {
                throw new FamilyException(GetType().Name, string.Format(StrUserFamilyOwner, family.User?.Name ?? targetId.ToString()));
            }

            await familyRepository.DeleteMemberAsync(targetId);
            return new GenericResponse(StrTitleAdvertise, StrFamilyMemberRemoved);
        }

        public async Task<bool> DeleteFamilyAsync(FamilyDeleteRequest request)
        {
            return await familyRepository.DeleteFamilyAsync(request.UserId, request.FamilyId);
        }

        public async Task<bool> DeleteFamilyByCreatorIdAsync(int userId)
        {
            return await familyRepository.DeleteFamilyByCreatorIdAsync(userId);
        }
    }
}
