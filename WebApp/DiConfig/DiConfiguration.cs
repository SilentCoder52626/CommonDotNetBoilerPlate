using BaseModule.Repository.User;
using DomainModule.BaseRepo;
using DomainModule.Repository;
using DomainModule.RepositoryInterface;
using DomainModule.RepositoryInterface.AuditLog;
using DomainModule.Service;
using DomainModule.ServiceInterface;
using InfrastructureModule.Repository;
using InfrastructureModule.Repository.AuditLog;
using ServiceModule.Service;
using System.Runtime.CompilerServices;


namespace WebApp.DiConfig
{
    public static class DiConfiguration
    {
        public static void  UseDIConfig(this IServiceCollection services)
        {
            UseRepository(services);
            UseService(services);
        }
        private static void UseRepository(IServiceCollection services)
        {
         services.AddScoped<UserRepositoryInterface,UserRepository>();
         services.AddScoped<RoleRepositoryInterface,RoleRepository>();
         services.AddScoped<IAuditLogRepository,AuditLogRepository>();
        }
        private static void UseService(IServiceCollection services)
        {
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddScoped<UserServiceInterface, UserService>();
            services.AddScoped<RoleServiceInterface, RoleService>();
        }
    }
}
