using System.ComponentModel.DataAnnotations.Schema;

namespace MyParentApi.DAL.Entities
{
    [Table("FamilyUsers")]
    public class ApiFamilyUser
    {
        public int Id { get; set; }
        public int FamilyId { get; set; }
        public int UserId { get; set; }

        public ApiUser User { get; set; }
    }
}
