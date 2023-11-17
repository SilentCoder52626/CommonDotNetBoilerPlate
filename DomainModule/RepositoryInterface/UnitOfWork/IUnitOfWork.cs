using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModule.RepositoryInterface
{
    public interface IUnitOfWork
    {
        Task<IDbContextTransaction> BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Unspecified);
        Task CompleteAsync();
    }
}
