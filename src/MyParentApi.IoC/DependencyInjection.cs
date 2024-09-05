using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyParentApi.Application.Interfaces;
using MyParentApi.Application.Services;
using MyParentApi.DAL;

namespace MyParentApi.IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServicesCollection(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<AppDbContext>(options => 
                options.UseSqlServer(connectionString)
            );

            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<ISysLogService, SysLogService>();

            return services;
        }
    }
}
