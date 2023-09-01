using DomainModule.Dto.ActivityLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModule.ServiceInterface.Account
{
    public interface IActivityLogService
    {
        Task Create(ActivityLogDto dto);
    }
}
