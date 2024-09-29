using Microsoft.EntityFrameworkCore;
using MyParentApi.DAL.Entities;
using MyParentApi.DAL.Interfaces;
using MyParentApi.Shared.Helpers.Exceptions;

namespace MyParentApi.DAL.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly AppDbContext context;

        public NotificationRepository(AppDbContext context) 
        {
            this.context = context;
        }

        public async Task<ICollection<ApiNotification>> GetNotesAsync(int userId)
        {
            return await context.Notifications
                .Include(x => x.User)
                .Include(x => x.Sender)
                .Where(x => x.UserId == userId)
                .ToListAsync();
        }

        public async Task<ICollection<ApiNotification>> GetNotesByTypeAsync(int userId, byte noteType = NoteTypeNormal)
        {
            return await context.Notifications
                .Include(x => x.User)
                .Include(x => x.Sender)
                .Where(x => x.UserId == userId && x.Type == noteType)
                .ToListAsync();
        }

        public async Task<bool> CreateNoteAsync(ApiUser sender, int targetId, string title, string description, string content, bool invite = false)
        {
            var notification = new ApiNotification()
            {
                UserId = targetId,
                SenderId = sender?.Id ?? 0,
                Title = title,
                Description = description,
                Content = content,
                Status = StatusNoteNew,
                Type = invite ? NoteTypeInvite : NoteTypeNormal,
                CreatedAt = DateTime.Now,
            };

            return await context.CreateAsync(notification);
        }

        public async Task<bool> ReadNoteAsync(int notificationId)
        {
            var notification = await context.Notifications.FirstOrDefaultAsync(x => x.Id == notificationId);
            if (notification == null)
            {
                throw new DatabaseException(GetType().Name, string.Format(StrNoteNotFound, notificationId));
            }

            notification.Status = StatusNoteRead;
            notification.ReadAt = DateTime.Now;
            return await context.UpdateAsync(notification);
        }

        public async Task<bool> DeleteNoteAsync(int notificationId, byte noteType = NoteTypeNormal)
        {
            var notification = await context.Notifications.FirstOrDefaultAsync(x => x.Id == notificationId);
            if (notification == null)
            {
                throw new DatabaseException(GetType().Name, string.Format(StrNoteNotFound, notificationId));
            }

            return await context.DeleteAsync(notification);
        }

        public async Task<bool> DeleteAllNotesAsync(int userId)
        {
            var notes = await context.Notifications.Where(x => x.UserId == userId).ToListAsync();
            if (notes == null || notes.Count == 0)
            {
                throw new DatabaseException(GetType().Name, StrNotesNotFound);
            }

            return await context.DeleteRangeAsync(notes);
        }
    }
}
