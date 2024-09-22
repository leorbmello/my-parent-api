using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MyParentApi.DAL.Entities
{
    [Table("Users")]
    public class ApiUser
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
        public string Name { get; set; }
        public byte Type { get; set; }
        public byte Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }

        public ApiUserInfo UserInfo { get; set; }

        [JsonIgnore]
        public ICollection<ApiUserRole> UserRoles { get; set; }
    }
}
