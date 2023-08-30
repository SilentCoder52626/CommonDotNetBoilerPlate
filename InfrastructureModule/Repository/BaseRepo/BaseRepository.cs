
using DomainModule.BaseRepo;
using InfrastructureModule.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureModule.Repository
{
    public class BaseRepository<T> : BaseRepositoryInterface<T> where T : class
    {
        private readonly AppDbContext _context;
        public BaseRepository(AppDbContext context)
        {
            _context = context;
        }
        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public async Task<IList<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync().ConfigureAwait(false);
        }
        public IList<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public async Task<T?> GetById(long id)
        {
            return await _context.Set<T>().FindAsync(id).ConfigureAwait(false);
        }

        public IQueryable<T> GetQueryable()
        {
            return _context.Set<T>();
        }

        public async Task InsertAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity).ConfigureAwait(false);
        }

        public async Task InsertRangeAsync(IList<T> entities)
        {
            await _context.Set<T>().AddRangeAsync(entities).ConfigureAwait(false);
        }

        public void Insert(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public void InsertRange(IList<T> entities)
        {
            _context.Set<T>().AddRange(entities);
        }

        public void Update(T entity)
        {
            _context.Set<T>().Attach(entity).State = EntityState.Modified;
        }
    }
}
