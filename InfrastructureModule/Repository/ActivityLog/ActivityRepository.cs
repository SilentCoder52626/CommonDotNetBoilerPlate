using DomainModule.RepositoryInterface.ActivityLog;
using InfrastructureModule.Context;

namespace InfrastructureModule.Repository.ActivityLog
{
    public class ActivityLogRepository : BaseRepository<DomainModule.Entity.Activity>, IActivityLogRepository
    {
        public ActivityLogRepository(AppDbContext context) : base(context)
        {

        }
    }
}
