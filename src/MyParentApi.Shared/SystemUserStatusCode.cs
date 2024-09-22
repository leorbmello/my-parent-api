namespace MyParentApi.Shared
{
    public class SystemUserStatusCode
    {
        public const byte 
            StatusActive = 1,
            StatusInactive = 2,
            StatusBanned = 3;


        public const byte
            RoleTypeAdmin = 1,
            RoleTypeUser = 2,
            RoleTypeChildren = 3;
    }
}
