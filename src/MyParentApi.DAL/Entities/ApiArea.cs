using System.ComponentModel.DataAnnotations.Schema;

namespace MyParentApi.DAL.Entities
{
    [Table("Areas")]
    public class ApiArea
    {
        public int Id { get; set; }
        public string Name { get; set; }


        public ICollection<ApiRolePermission> RolePermissions { get; set; }
    }
}
