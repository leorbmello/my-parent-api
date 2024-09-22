using Microsoft.Extensions.Logging;
using MyParentApi.Application.Interfaces;
using MyParentApi.DAL.Entities;

namespace MyParentApi.Application.Services
{
    public class SysLogService : ISysLogService
    {
        private readonly ILogger<SysLogService> logger;
        private readonly AppDbContext context;

        public SysLogService(
            ILogger<SysLogService> logger,
            AppDbContext context)
        {
            this.logger = logger;
            this.context = context;
        }

        public async Task SaveUserLogAsync(int userId, string operation, string data)
        {
            try
            {
                await context.CreateAsync(new UserLogOper()
                {
                    UserId = userId,
                    Operation = operation,
                    JsonText = data,
                    Date = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex.ToString());
            }
        }

        public async Task SaveSysLogAsync(int userId, string operation, string data, string exception = "")
        {
            try
            {
                await context.CreateAsync(new SysLogOper()
                {
                    UserId = userId,
                    Operation = operation,
                    JsonText = data,
                    ErrorText = exception,
                    Date = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex.ToString());
            }
        }
    }
}