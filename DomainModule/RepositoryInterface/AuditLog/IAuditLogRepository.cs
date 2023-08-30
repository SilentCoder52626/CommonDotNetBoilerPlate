using DomainModule.BaseRepo;
using DomainModule.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModule.RepositoryInterface.AuditLog
{
    public interface IAuditLogRepository : BaseRepositoryInterface<Audit>
    {
    }
}
