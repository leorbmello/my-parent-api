using Microsoft.Extensions.Logging;
using MyParentApi.Application.DTOs.Requests.Family;
using MyParentApi.Application.DTOs.Responses;
using MyParentApi.Application.Interfaces;
using MyParentApi.DAL.Entities;
using MyParentApi.DAL.Interfaces;

namespace MyParentApi.Application.Services
{
    public class FamilyService : IFamilyService
    {
        private readonly ILogger<FamilyService> logger;
        private readonly IFamilyRepository familyRepository;
        private readonly IUserRepository userRepository;
        private readonly INotificationRepository notificationRepository;

        public FamilyService(
            ILogger<FamilyService> logger,
            IFamilyRepository familyRepository,
            IUserRepository userRepository,
            INotificationRepository notificationRepository)
        {
            this.logger = logger;
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
            try
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

                throw new Exception("Usuário já pertence à uma família!");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                return null;
            }
        }

        public async Task<ApiFamily> AcceptFamilyInviteAsync(ApiUser user)
        {
            try
            {
                var notes = await notificationRepository.GetNotesByTypeAsync(user.Id, NoteTypeInvite);
                if (notes.Count < 1)
                {
                    throw new Exception("Não há nenhum invite para entrar em uma família!");
                }

                var note = notes.FirstOrDefault();
                if (note != null)
                {
                    if (!await familyRepository.JoinFamilyAsync(note.FamilyId, user.Id))
                    {
                        throw new Exception("Não foi possível entrar na família.");
                    }
                }

                var family = await familyRepository.GetFamilyByUserIdAsync(user.Id);
                if (family != null)
                {
                    throw new Exception("Família não encontrada.");
                }

                return family;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw new SystemException(ex.ToString());
            }
        }

        public async Task<GenericResponse> DeleteFamilyMemberAsync(ApiUser user, int targetId)
        {
            try
            {
                var family = await familyRepository.GetFamilyByUserIdAsync(user.Id);
                if (family == null)
                {
                    throw new Exception("Família não encontrada.");
                }

                if (family.UserCreatorId != user.Id)
                {
                    throw new Exception("Usuário não é um proprietário da família, membro não pode ser removido!");
                }

                if (family.UserCreatorId == targetId)
                {
                    throw new Exception("Usuário o proprietário da família, membro não pode ser removido!");
                }

                await familyRepository.DeleteMemberAsync(targetId);
                return new GenericResponse("Aviso", "Membero foi removido da família com sucesso!");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw new SystemException(ex.ToString());
            }
        }

        public async Task<bool> DeleteFamilyAsync(FamilyDeleteRequest request)
        {
            try
            {
                return await familyRepository.DeleteFamilyAsync(request.UserId, request.FamilyId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw new SystemException(ex.ToString());
            }
        }

        public async Task<bool> DeleteFamilyByCreatorIdAsync(int userId)
        {
            try
            {
                return await familyRepository.DeleteFamilyByCreatorIdAsync(userId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw new SystemException(ex.ToString());
            }
        }
    }
}
