using MyParentApi.DAL.Entities;

namespace MyParentApi.Application.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(ApiUser user);
    }
}
