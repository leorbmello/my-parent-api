using System.ComponentModel.DataAnnotations.Schema;

namespace MyParentApi.DAL.Entities
{
    [Table("Families")]
    public class ApiFamily
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int UserCreatorId { get; set; }
        public byte Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }

        public ApiUser User { get; set; }
    }
}
