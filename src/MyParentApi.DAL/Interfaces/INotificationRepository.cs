using MyParentApi.DAL.Entities;

namespace MyParentApi.DAL.Interfaces
{
    public interface INotificationRepository
    {
        Task<ICollection<ApiNotification>> GetNotesAsync(int userId);
        Task<ICollection<ApiNotification>> GetNotesByTypeAsync(int userId, byte noteType = NoteTypeNormal);
        Task<bool> CreateNoteAsync(ApiUser sender, int targetId, string title, string description, string content, bool invite = false);
        Task<bool> ReadNoteAsync(int notificationId);
        Task<bool> DeleteNoteAsync(int notificationId, byte noteType = NoteTypeNormal);
        Task<bool> DeleteAllNotesAsync(int userId);
    }
}
