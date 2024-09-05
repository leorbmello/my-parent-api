using Microsoft.EntityFrameworkCore;
using MyParentApi.Application.Interfaces;

namespace MyParentApi.Application.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly AppDbContext context;

        public PermissionService(AppDbContext context)
        {
            this.context = context;
        }

        public bool CanView(string email, string area)
        {
            return HasPermission(email, area, AreaOption_View);
        }

        public bool CanEdit(string email, string area)
        {
            return HasPermission(email, area, AreaOption_Edit);
        }

        public bool CanCreate(string email, string area)
        {
            return HasPermission(email, area, AreaOption_Create);
        }

        public bool CanDelete(string email, string area)
        {
            return HasPermission(email, area, AreaOption_Delete);
        }

        public bool HasPermission(string userEmail, string areaName, string permissionName)
        {
            var user = context.Users
              .Include(u => u.UserRoles)
                  .ThenInclude(ur => ur.Role)
                      .ThenInclude(r => r.RolePermissions)
                          .ThenInclude(rp => rp.Area)
              .Include(u => u.UserRoles)
                  .ThenInclude(ur => ur.Role)
                      .ThenInclude(r => r.RolePermissions)
                          .ThenInclude(rp => rp.Permission)
              .FirstOrDefault(u => u.Email == userEmail);

            if (user == null)
            {
                return false;
            }

            return user.UserRoles.Any(ur => ur.Role.RolePermissions
                .Any(rp => rp.Area.Name == areaName && rp.Permission.Name == permissionName));
        }
    }
}
