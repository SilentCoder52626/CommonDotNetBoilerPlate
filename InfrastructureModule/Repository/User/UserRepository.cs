using DomainModule.Entity;
using DomainModule.RepositoryInterface;
using InfrastructureModule.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureModule.Repository
{
    public class UserRepository:BaseRepository<User>,UserRepositoryInterface
    {
        public UserRepository(AppDbContext context):base(context)
        {
            
        }
        public async Task<User?> GetByEmail(string email)
        {
            return await GetQueryable().Where(a => a.Email == email).SingleOrDefaultAsync().ConfigureAwait(false);
        }

        public async Task<User?> GetByIdStringAsync(string Id)
        {
            return await GetQueryable().Where(a => a.Id == Id).SingleOrDefaultAsync().ConfigureAwait(false);
        }
          public User? GetByIdString(string Id)
        {
            return GetQueryable().Where(a => a.Id == Id).SingleOrDefault();
        }

        public async Task<User?> GetByMobile(string mobile)
        {
            return await GetQueryable().Where(a => a.PhoneNumber == mobile).SingleOrDefaultAsync().ConfigureAwait(false);
        }

        public async Task<User?> GetByUserName(string userName)
        {
            return await GetQueryable().Where(a => a.UserName == userName).SingleOrDefaultAsync().ConfigureAwait(false);
        }
    }
}
