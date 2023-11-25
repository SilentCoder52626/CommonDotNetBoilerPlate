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
        Task<IDbContextTransaction> BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.Unspecified);
        IDbContextTransaction BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Unspecified);
        Task CompleteAsync();
        void Complete();
    }
}
