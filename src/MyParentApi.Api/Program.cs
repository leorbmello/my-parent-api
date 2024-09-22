using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MyParentApi.DAL;
using MyParentApi.IoC;
using Newtonsoft.Json.Linq;
using System.Text;

namespace MyParentApi.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Adicione a configuração de autenticação JWT
            var jwtSettings = builder.Configuration.GetSection("JwtSettings");
            var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero
                };
                options.Events = new JwtBearerEvents()
                {
                    OnChallenge = context =>
                    {
                        // Skip the default logic.
                        context.HandleResponse();
                        var payload = new JObject
                        {
                            ["title"] = "Sessão Expirada",
                            ["erroInformal"] = "Sessão expirada devido a inatividade ou tempo limite de acesso. Será necessário fazer o acesso novamente.",
                            ["erroFormal"] = "Token Invalido ou Usuario não Autenticado"
                        };

                        context.Response.ContentType = "application/json";
                        context.Response.StatusCode = 401;

                        return context.Response.WriteAsync(payload.ToString());
                    }
                };
            });

            // Add services to the container.
            builder.Services.AddServicesCollection(builder.Configuration);
            builder.Services.AddAuthorization(options =>
            {
                // Atribuir policies dinamicamente do banco de dados
                using (var scope = builder.Services.BuildServiceProvider().CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    var roles = dbContext.Roles.ToList();

                    foreach (var role in roles)
                    {
                        options.AddPolicy(role.Name, policy => policy.RequireRole(role.Name));
                    }
                }
            });

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
