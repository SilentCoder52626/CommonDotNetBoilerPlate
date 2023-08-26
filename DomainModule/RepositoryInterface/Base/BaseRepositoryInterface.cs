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
        Task InsertAsync(T entity);
        Task InsertRange(IList<T> entities);
        void Update(T entity);
        void Delete(T entity);
        IQueryable<T> GetQueryable();
        Task<T?> GetById(long id);
    }
}
