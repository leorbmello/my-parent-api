namespace MyParentApi.Application.DTOs.Responses
{
    public record CreateUserResponse
    {
        public CreateUserResponse() 
        { 
        }

        public CreateUserResponse(string email, string name)
        {
            Email = email;
            Name = name;
        }

        public string Email { get; init; }
        public string Name { get; init; }
    }
}
