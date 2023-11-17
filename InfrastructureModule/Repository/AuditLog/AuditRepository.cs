using DomainModule.Entity;
using DomainModule.RepositoryInterface.ActivityLog;
using DomainModule.RepositoryInterface.AuditLog;
using InfrastructureModule.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureModule.Repository.AuditLog
{
    public class AuditLogRepository : BaseRepository<Audit>, IAuditLogRepository
    {
        public AuditLogRepository(AppDbContext context) : base(context)
        {

        }
    }
}
