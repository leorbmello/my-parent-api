namespace MyParentApi.Shared
{
    public class SystemRoleName
    {
        public const string 
            RoleAdmin = "Admin",
            RoleParent = "User",
            RoleChildren = "Children";

        public const byte
            RoleAdminId = 1,
            RoleUserId = 2,
            RoleChildrenId = 3;
    }
}
