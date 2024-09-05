using System.ComponentModel.DataAnnotations.Schema;

namespace MyParentApi.DAL.Entities
{
    [Table("RolePermissions")]
    public class ApiRolePermission
    {
        public int Id { get; set; }

        public int RoleId { get; set; }
        public ApiRole Role { get; set; }

        public int PermissionId { get; set; }
        public ApiPermission Permission { get; set; }

        public int AreaId { get; set; }
        public ApiArea Area { get; set; }
    }
}
