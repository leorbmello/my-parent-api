using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyParentApi.Application.DTOs.Requests.Notification;
using MyParentApi.DAL.Interfaces;

namespace MyParentApi.Api.Controllers
{
    /// <summary>
    /// Esta classe é apenas um teste, os códigos são basicos para testar a funcionalidade das notificações,
    /// visto que elas não precisam de uma controladora, seriam disparadas de acordo com as demais funcionalidades
    /// da API.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationRepository notificationRepository;
        private readonly IUserRepository userRepository;

        public NotificationController(
            IUserRepository userRepository,
            INotificationRepository notificationRepository)
        {
            this.userRepository = userRepository;
            this.notificationRepository = notificationRepository;
        }

        [HttpGet("query-all/{userId}")]
        public async Task<IActionResult> GetAllNotesAsync(int userId)
        {
            return Ok(await notificationRepository.GetNotesAsync(userId));
        }

        [HttpGet("query-invite/{userId}")]
        public async Task<IActionResult> GetNotesAsync(int userId)
        {
            return Ok(await notificationRepository.GetNotesByTypeAsync(userId, NoteTypeInvite));
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateNoteAsync([FromBody] NotificationRequest request)
        {
            var sender = await userRepository.GetUserByIdAsync(request.SenderId);
            var target = await userRepository.GetUserByIdAsync(request.TargetId);
            if (target == null)
            {
                throw new Exception("Usuário alvo não encontrado");
            }

            bool result = await notificationRepository.CreateNoteAsync(
                sender,
                request.TargetId,
                request.Title,
                request.Description,
                request.Content,
                request.Type == NoteTypeInvite);

            if (result)
            {
                return Ok("Notificação criada com sucesso!");
            }
            else
            {
                return Ok("Fez cagada seu puto!");
            }
        }
    }
}
