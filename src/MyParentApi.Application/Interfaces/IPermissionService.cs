namespace MyParentApi.Application.Interfaces
{
    public interface IPermissionService
    {
        bool CanView(string email, string area);
        bool CanCreate(string email, string area);
        bool CanEdit(string email, string area);
        bool CanDelete(string email, string area);
        bool HasPermission(string userEmail, string areaName, string permissionName);
    }
}
