using DomainModule.BaseRepo;
using DomainModule.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModule.RepositoryInterface
{
    public interface UserRepositoryInterface:BaseRepositoryInterface<User>
    {
        Task<User?> GetByMobile(string mobile);
        Task<User?> GetByEmail(string email);
        Task<User?> GetByIdStringAsync(string Id);
        User? GetByIdString(string Id);
        Task<User?> GetByUserName(string name);

    }
}
