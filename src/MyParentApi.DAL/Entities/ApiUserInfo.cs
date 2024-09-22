using System.ComponentModel.DataAnnotations.Schema;

namespace MyParentApi.DAL.Entities
{
    [Table("UserInfo")]
    public class ApiUserInfo
    {
        public int Id { get; set; }
        public string FirstName {  get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public byte Gender { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ChangedAt { get; set; }

        public ApiUser User { get; set; }
    }
}
