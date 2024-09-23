using System.ComponentModel.DataAnnotations;

namespace MyParentApi.Application.DTOs.Requests.Notification
{
    public record NotificationRequest
    {
        public NotificationRequest(int senderId, int targetId, string title, string description, string content, byte type)
        {
            SenderId = senderId;
            TargetId = targetId;
            Title = title;
            Description = description;
            Content = content;
            Type = type;
        }

        public int SenderId { get; set; }
        [Required] public int TargetId { get; set; }
        [Required] public string Title { get; init; }
        [Required] public string Description { get; init; }
        [Required] public string Content { get; init; }
        public byte Type { get; set; }
    }
}
