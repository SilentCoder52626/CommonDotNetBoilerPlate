
using DomainModule.Repository;
using InfrastructureModule.Context;
using InfrastructureModule.Repository;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
namespace BaseModule.Repository.User
{
  public  class RoleRepository:BaseRepository<IdentityRole>, RoleRepositoryInterface
    {
        public RoleRepository(AppDbContext dbContext) : base(dbContext)
        {

        }

    }
}
