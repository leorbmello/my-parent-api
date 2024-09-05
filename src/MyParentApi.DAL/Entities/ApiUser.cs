using System.ComponentModel.DataAnnotations.Schema;

namespace MyParentApi.DAL.Entities
{
    [Table("Users")]
    public class ApiUser
    {
        /*  Id INT PRIMARY KEY IDENTITY,
            Email NVARCHAR(256) NOT NULL,
            PasswordHash NVARCHAR(256) NOT NULL,
	        Nome NVARCHAR(256) NOT NULL,
	        Tipo TINYINT NOT NULL,
	        Status TINYINT NOT NULL,
	        DataNascimento DATETIME NOT NULL,
	        DataCriacao DATETIME NOT NULL,
	        DataAlteracao DATETIME NOT NULL,
        */
        public int Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }// adiciono uma chave dinamica ou mantenho fixa?
        public string Nome { get; set; }
        public byte Tipo { get; set; }
        public byte Status { get; set; }
        public DateTime DataNascimento { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataAlteracao { get; set; }

        public ICollection<ApiUserRole> UserRoles { get; set; }
    }
}
