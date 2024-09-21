using System.ComponentModel.DataAnnotations.Schema;

namespace MyParentApi.DAL.Entities
{
    [Table("Roles")]
    public class ApiRole
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<ApiUserRole> UserRoles { get; set; }
    }
}
