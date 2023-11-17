using DomainModule.Dto.ActivityLog;
using DomainModule.Entity;
using DomainModule.RepositoryInterface;
using DomainModule.RepositoryInterface.ActivityLog;
using DomainModule.ServiceInterface.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceModule.Service
{
    public class ActivityLogService : IActivityLogService
    {
        private readonly IActivityLogRepository _activitytLogRepo;
        private readonly IUnitOfWork _unitOfWork;

        public ActivityLogService(IActivityLogRepository activitytLogRepo, IUnitOfWork unitOfWork)
        {
            _activitytLogRepo = activitytLogRepo;
            _unitOfWork = unitOfWork;
        }

        public async Task Create(ActivityLogDto dto)
        {
            using var tx = await _unitOfWork.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
            try
            {
                var activityLog = new Activity()
                {
                    ActionName = dto.ActionName,
                    ActionOn = dto.ActionOn,
                    Area = dto.Area,
                    IpAddress = dto.IpAddress,
                    PageAccessed = dto.PageAccessed,
                    Browser = dto.Browser,
                    ControllerName = dto.ControllerName,
                    Data = dto.Data,
                    QueryString = dto.QueryString,
                    SessionId = dto.SessionId,
                    Status = dto.Status,
                    UrlReferrer = dto.UrlReferrer,
                    UserId = dto.UserId,
                    UserName = dto.UserName
                };
                await _activitytLogRepo.InsertAsync(activityLog).ConfigureAwait(false);
                await _unitOfWork.CompleteAsync().ConfigureAwait(false);
                await tx.CommitAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                await tx.RollbackAsync().ConfigureAwait(false);
                throw;
            }
        }
    }
}
