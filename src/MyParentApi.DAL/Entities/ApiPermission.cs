using System.ComponentModel.DataAnnotations.Schema;

namespace MyParentApi.DAL.Entities
{
    [Table("Permissions")]
    public class ApiPermission
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<ApiRolePermission> RolePermissions { get; set; }
    }
}
