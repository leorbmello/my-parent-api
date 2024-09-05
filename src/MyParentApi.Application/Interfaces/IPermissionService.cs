namespace MyParentApi.Application.Interfaces
{
    public interface IPermissionService
    {
        bool HasPermission(string userEmail, string areaName, string permissionName);
    }
}
