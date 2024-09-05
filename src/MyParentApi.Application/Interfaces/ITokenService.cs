namespace MyParentApi.Application.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(string email);
    }
}
