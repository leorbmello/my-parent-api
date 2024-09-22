using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using MyParentApi.Application.Interfaces;
using System.Security.Claims;
using System.Text;

namespace MyParentApi.Application.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration configuration;
        private readonly AppDbContext context;

        public TokenService(
            IConfiguration configuration, 
            AppDbContext context)
        {
            this.configuration = configuration;
            this.context = context;
        }

        public string GenerateToken(string email)
        {
            var user = context.Users
                .Include(x => x.UserRoles)
                .ThenInclude(x => x.Role)
                .FirstOrDefault(x => x.Email.Equals(email));
            
            if (user == null)
            {
                throw new UnauthorizedAccessException("Credenciais inválidas!");
            }

            var jwtSettings = configuration.GetSection("JwtSettings");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email)
            };

            // adicionar as roles como claims para serem reconhecidas
            claims.AddRange(user.UserRoles.Select(role => new Claim(ClaimTypes.Role, role.Role.Name)));

            // Criando a definição de token com JsonWebTokenHandler
            var tokenHandler = new JsonWebTokenHandler();

            // Parâmetros do token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = creds
            };

            // Gerar o token
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return token; // O token retornado tem o formato JWT
        }
    }
}
