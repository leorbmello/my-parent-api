using System.ComponentModel.DataAnnotations.Schema;

namespace MyParentApi.DAL.Entities
{
    [Table("UserRoles")]
    public class ApiUserRole
    {
        public int UserId { get; set; }
        public ApiUser User { get; set; }

        public int RoleId { get; set; }
        public ApiRole Role { get; set; }
    }
}
