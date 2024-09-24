using System.ComponentModel.DataAnnotations;

namespace MyParentApi.Application.DTOs.Requests.Family
{
    public record FamilyCreateRequest
    {
        public FamilyCreateRequest(string email, string name)
        {
            Email = email;
            Name = name;
        }

        [Required] public string Email { get; init; }
        [Required] public string Name { get; init; }
    }
}
