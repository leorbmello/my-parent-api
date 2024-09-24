using System.ComponentModel.DataAnnotations;

namespace MyParentApi.Application.DTOs.Requests.Family
{
    public record FamilyDeleteRequest
    {
        public FamilyDeleteRequest(int userId, int familyId)
        {
            UserId = userId;
            FamilyId = familyId;
        }

        [Required] public int UserId { get; set; }
        [Required] public int FamilyId { get; set; }
    }
}
