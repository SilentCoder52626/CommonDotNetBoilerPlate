using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModule.BaseRepo
{
   public  interface BaseRepositoryInterface<T> where T : class
    {
        Task<IList<T>> GetAllAsync();
        IList<T> GetAll();
        Task InsertAsync(T entity);
        Task InsertRangeAsync(IList<T> entities);
        void Update(T entity);
        void Delete(T entity);
        IQueryable<T> GetQueryable();
        Task<T?> GetById(long id);
    }
}
