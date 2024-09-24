using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyParentApi.Application.Interfaces;
using MyParentApi.Application.Services;
using MyParentApi.DAL;
using MyParentApi.DAL.Interfaces;
using MyParentApi.DAL.Repositories;

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
            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<IFamilyService, FamilyService>();
            services.AddScoped<ISysLogService, SysLogService>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IFamilyRepository, FamilyRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();

            return services;
        }
    }
}
