using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyParentApi.DAL.Entities;

namespace MyParentApi.DAL
{
    public class AppDbContext : DbContext
    {
        private readonly ILogger<AppDbContext> logger;

        public virtual DbSet<ApiUser> Users { get; set; }
        public virtual DbSet<ApiRole> Roles { get; set; }
        public virtual DbSet<ApiArea> Areas { get; set; }
        public virtual DbSet<ApiPermission> Permissions { get; set; }
        public virtual DbSet<ApiRolePermission> RolePermissions { get; set; }
        public virtual DbSet<SysLogOper> SysLogOpers { get; set; }
        public virtual DbSet<UserLogOper> UserLogOpers { get; set; }
                
        public AppDbContext(ILogger<AppDbContext> logger, DbContextOptions<AppDbContext> options)
            : base(options)
        {
            this.logger = logger;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApiArea>()
                .HasMany(a => a.RolePermissions)
                .WithOne(rp => rp.Area)
                .HasForeignKey(rp => rp.AreaId);

            modelBuilder.Entity<ApiRole>()
                .HasMany(r => r.RolePermissions)
                .WithOne(rp => rp.Role)
                .HasForeignKey(rp => rp.RoleId);

            modelBuilder.Entity<ApiUser>()
                .HasMany(u => u.UserRoles)
                .WithOne(ur => ur.User)
                .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<ApiRole>()
                .HasMany(r => r.UserRoles)
                .WithOne(ur => ur.Role)
                .HasForeignKey(ur => ur.RoleId);

            modelBuilder.Entity<ApiUserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });
        }

        public async Task<bool> CreateAsync<T>(T entity, CancellationToken cancellationToken = default)
            where T : class
        {
            try
            {
                this.Add<T>(entity);
                await SaveChangesAsync(cancellationToken);
                return true;
            }
            catch (Exception ex)
            {
                logger.LogCritical("CreateAsync()-> Error: {}", ex.ToString());
                return false;
            }
        }

        public async Task<bool> CreateRangeAsync<T>(IEnumerable<T> entities, CancellationToken cancellationToken = default)
            where T : class
        {
            try
            {
                foreach (var entity in entities)
                {
                    this.Add<T>(entity);
                }

                await SaveChangesAsync(cancellationToken);
                return true;
            }
            catch (Exception ex)
            {
                logger.LogCritical("CreateRangeAsync()-> Error: {}", ex.ToString());
                return false;
            }
        }

        public async Task<bool> UpdateAsync<T>(T entity, CancellationToken cancellationToken = default)
            where T : class
        {
            try
            {
                this.Update<T>(entity);
                await SaveChangesAsync(cancellationToken);
                return true;
            }
            catch (Exception ex)
            {
                logger.LogCritical("UpdateAsync()-> Error: {}", ex.ToString());
                return false;
            }
        }

        public async Task<bool> UpdateRangeAsync<T>(IEnumerable<T> entities, CancellationToken cancellationToken = default)
            where T : class
        {
            try
            {
                foreach (var entity in entities)
                {
                    this.Update<T>(entity);
                }

                await SaveChangesAsync(cancellationToken);
                return true;
            }
            catch (Exception ex)
            {
                logger.LogCritical("UpdateRangeAsync()-> Error: {}", ex.ToString());
                return false;
            }
        }

        public async Task<bool> DeleteAsync<T>(T entity, CancellationToken cancellationToken = default)
            where T : class
        {
            try
            {
                this.Remove<T>(entity);
                await SaveChangesAsync(cancellationToken);
                return true;
            }
            catch (Exception ex)
            {
                logger.LogCritical("DeleteAsync()-> Error: {}", ex.ToString());
                return false;
            }
        }

        public async Task<bool> DeleteRangeAsync<T>(IEnumerable<T> entities, CancellationToken cancellationToken = default)
            where T : class
        {
            try
            {
                foreach (var entity in entities)
                {
                    this.Remove<T>(entity);
                }

                await SaveChangesAsync(cancellationToken);
                return true;
            }
            catch (Exception ex)
            {
                logger.LogCritical("DeleteRangeAsync()-> Error: {}", ex.ToString());
                return false;
            }
        }
    }
}
