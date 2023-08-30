using DomainModule.RepositoryInterface;
using InfrastructureModule.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureModule.Repository
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly AppDbContext _context;
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IDbContextTransaction> BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Unspecified)
        {
            return await _context.Database.BeginTransactionAsync(isolationLevel);
        }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }
        public void Complete()
        {
            _context.SaveChanges();
        }
    }
}
