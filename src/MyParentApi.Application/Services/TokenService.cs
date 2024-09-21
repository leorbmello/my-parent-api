using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MyParentApi.Application.Interfaces;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
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
            }; 

            claims.Add(new Claim(JwtRegisteredClaimNames.UniqueName, user.Id.ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));

            // adicionar as roles como claims para serem reconhecidas
            //claims.AddRange(user.UserRoles.Select(role => new Claim(ClaimTypes.Role, role.Role.Name)));
            var expiration = DateTime.UtcNow.AddHours(23);
            var token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: expiration,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
