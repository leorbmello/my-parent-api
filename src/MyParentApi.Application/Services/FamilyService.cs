using Microsoft.Extensions.Logging;
using MyParentApi.Application.DTOs.Responses;
using MyParentApi.DAL.Entities;
using MyParentApi.DAL.Interfaces;

namespace MyParentApi.Application.Services
{
    public class FamilyService
    {
        private readonly ILogger<FamilyService> logger;
        private readonly IFamilyRepository familyRepository;
        private readonly INotificationRepository notificationRepository;

        public FamilyService(
            ILogger<FamilyService> logger,
            IFamilyRepository familyRepository,
            INotificationRepository notificationRepository)
        {
            this.logger = logger;
            this.familyRepository = familyRepository;
            this.notificationRepository = notificationRepository;
        }

        public async Task<ApiFamily> CreateNewFamilyAsync(ApiUser user, string name)
        {
            try
            {
                var family = familyRepository.GetFamilyByUserIdAsync(user.Id);
                if (family == null) 
                { 
                    return await familyRepository.CreateFamilyAsync(user, name);
                }

                throw new Exception("Usuário já pertence à uma família!");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw new SystemException(ex.ToString());
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

        public async Task<bool> DeleteFamilyAsync(int familyId)
        {
            try
            {
                return await familyRepository.DeleteFamilyAsync(familyId);
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
