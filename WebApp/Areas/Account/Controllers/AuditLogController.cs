using DomainModule.RepositoryInterface;
using DomainModule.RepositoryInterface.ActivityLog;
using DomainModule.RepositoryInterface.AuditLog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Areas.Account.ViewModel;

namespace WebApp.Areas.Account.Controllers
{
    [Area("Account")]
    public class AuditLogController : Controller
    {
        private readonly IAuditLogRepository _auditLogRepo;
        private readonly UserRepositoryInterface _userRepo;
        public AuditLogController(IAuditLogRepository auditLogRepo,
            UserRepositoryInterface userRepo)
        {
            _auditLogRepo = auditLogRepo;
            _userRepo = userRepo;
        }
        [Authorize(Policy = "AuditLog-View")]
        public IActionResult Index()
        {
            var auditLogs = _auditLogRepo.GetAll();
            var auditLogIndexModel = new List<AuditLogViewModel>();
            foreach (var log in auditLogs)
            {
                auditLogIndexModel.Add(new AuditLogViewModel
                {
                    Id = log.Id,
                    UserName = (_userRepo.GetByIdString(log.UserId))?.Name ?? "",
                    TableName = log.TableName,
                    OldValues = log.OldValues,
                    NewValues = log.NewValues,
                    AffectedColumns = log.AffectedColumns,
                    Type = log.Type,
                    ActionOn = log.DateTime.ToString("yyyy-MM-dd hh:mm tt")
                });
            }
            return View(auditLogIndexModel);
        }
    }
}
