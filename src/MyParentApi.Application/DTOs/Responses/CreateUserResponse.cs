namespace MyParentApi.Application.DTOs.Responses
{
    public record CreateUserResponse
    {
        public CreateUserResponse() 
        { 
        }

        public CreateUserResponse(string email, string name, string password, string salt)
        {
            Email = email;
            Name = name;
            Password = password;
            Salt = salt;
        }

        public string Email { get; init; }
        public string Name { get; init; }
        public string Password { get; init; }
        public string Salt { get; init; }
    }
}
