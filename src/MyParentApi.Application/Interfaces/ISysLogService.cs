namespace MyParentApi.Application.Interfaces
{
    public interface ISysLogService
    {
        Task SaveUserLogAsync(int userId, string operation, string data);
        Task SaveSysLogAsync(int userId, string operation, string data, string exception = "");
    }
}
