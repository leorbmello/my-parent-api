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

        public async Task<ICollection<ApiNotification>> GetNotesByTypeAsync(int userId, byte type)
        {
            return await context.Notifications
                .Include(x => x.User)
                .Include(x => x.Sender)
                .Where(x => x.UserId == userId && x.Type == type)
                .ToListAsync();
        }

        public async Task<bool> CreateNoteAsync(ApiUser sender, int targetId, string title, string description, string content, bool invite = false)
        {
            try
            {
                var notification = new ApiNotification()
                {
                    UserId = targetId,
                    SenderId = sender?.Id ?? 0,
                    Title = title,
                    Description = description,
                    Content = content,
                    Status = StatusNoteNew,
                    CreatedAt = DateTime.Now,
                };

                return await context.CreateAsync(notification);
            }
            catch (Exception ex)
            {
                throw new DatabaseException(GetType().Name, ex);
            }
        }

        public async Task<bool> ReadNoteAsync(int notificationId)
        {
            try
            {
                var notification = await context.Notifications.FirstOrDefaultAsync(x => x.Id == notificationId);
                if (notification == null)
                {
                    return false;
                }

                notification.Status = StatusNoteRead;
                notification.ReadAt = DateTime.Now;
                return await context.UpdateAsync(notification);
            }
            catch (Exception ex)
            {
                throw new DatabaseException(GetType().Name, ex);
            }
        }

        public async Task<bool> DeleteNoteAsync(int notificationId)
        {
            try
            {
                var notification = await context.Notifications.FirstOrDefaultAsync(x => x.Id == notificationId);
                if (notification == null)
                {
                    return false;
                }

                return await context.DeleteAsync(notification);
            }
            catch (Exception ex)
            {
                throw new DatabaseException(GetType().Name, ex);
            }
        }

        public async Task<bool> DeleteAllNotesAsync(int userId)
        {
            try
            {
                var notes = await context.Notifications.Where(x => x.UserId == userId).ToListAsync();
                if (notes == null || notes.Count == 0)
                {
                    return false;
                }

                return await context.DeleteRangeAsync(notes);
            }
            catch (Exception ex)
            {
                throw new DatabaseException(GetType().Name, ex);
            }
        }
    }
}
