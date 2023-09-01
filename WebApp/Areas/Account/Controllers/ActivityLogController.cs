using DomainModule.RepositoryInterface;
using DomainModule.RepositoryInterface.ActivityLog;
using InfrastructureModule.Repository.ActivityLog;
using InfrastructureModule.Repository.AuditLog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLog.Fluent;
using WebApp.Areas.Account.ViewModel;

namespace WebApp.Areas.Account.Controllers
{
    [Area("Account")]
    public class ActivityLogController : Controller
    {
        private readonly IActivityLogRepository _activityLogRepo;
        private readonly UserRepositoryInterface _userRepo;
        public ActivityLogController(IActivityLogRepository ActivityLogRepo,
            UserRepositoryInterface userRepo)
        {
            _activityLogRepo = ActivityLogRepo;
            _userRepo = userRepo;
        }
        [Authorize(Policy = "ActivityLog-View")]
        public async Task<IActionResult> Index()
        {
            var activityLogs = await _activityLogRepo.GetQueryable().OrderByDescending(a => a.Id).ToListAsync();
            var activityLogModel = new List<ActivityLogViewModel>();
            foreach (var activityLog in activityLogs)
            {
                activityLogModel.Add(new ActivityLogViewModel
                {
                    Area = activityLog.Area,
                    ActionName = activityLog.ActionName,
                    ControllerName = activityLog.ControllerName,
                    IpAddress = activityLog.IpAddress,
                    PageAccessed = activityLog.PageAccessed,
                    SessionId = activityLog.SessionId,
                    Browser = activityLog.Browser,
                    UrlReferrer = activityLog.UrlReferrer,
                    UserName = (_userRepo.GetByIdString(activityLog.UserId))?.Name ?? "",
                    UserId = activityLog.UserId,
                    Status = activityLog.Status,
                    Data = activityLog.Data,
                    ActionOn = activityLog.ActionOn.ToString("yyyy-MM-dd hh:mm tt")
                });

            }
            return View(activityLogModel);
        }
    }
}
