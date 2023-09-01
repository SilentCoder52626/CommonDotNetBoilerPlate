using DomainModule.BaseRepo;
using DomainModule.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModule.RepositoryInterface.ActivityLog
{
    public interface IActivityLogRepository : BaseRepositoryInterface<DomainModule.Entity.Activity>
    {
    }
}
