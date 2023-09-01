using DomainModule.Dto.AuditDto;
using DomainModule.Entity;
using DomainModule.Enums;
using InfrastructureModule.Mapping;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureModule.Context
{
    public class AppDbContext:IdentityDbContext
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }
        public DbSet<Audit> Audits { get; set; }
        public DbSet<Activity> ActivityLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new UserEntityMapping());
            builder.ApplyConfiguration(new AuditEntityMapping());
            builder.ApplyConfiguration(new ActivityEntityMapping());
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var conString = _configuration.GetConnectionString("DefaultConnection");
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql(conString, ServerVersion.AutoDetect(conString));
            }
        }
        public virtual async Task<int> SaveChangesAsync()
        {
            OnBeforeSaveChanges();
            var result = await base.SaveChangesAsync();
            return result;
        }
        public override int SaveChanges()
        {
            OnBeforeSaveChanges();
            var result = base.SaveChanges();
            return result;
        }

        private void OnBeforeSaveChanges()
        {
            var userId = "";
            if (_httpContextAccessor.HttpContext.User.Claims.Count() > 0)
            {
                userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            }
            var ipAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            var browser = _httpContextAccessor.HttpContext.Request.Headers["user-agent"].ToString();
            ChangeTracker.DetectChanges();
            var auditDatas = new List<AuditDto>();
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is Audit || entry.Entity is Activity || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                    continue;
                var model = new AuditDto(entry);
                model.TableName = entry.Entity.GetType().Name;
                model.UserId = userId;
                model.IpAddress = ipAddress;
                model.Browser = browser;
                auditDatas.Add(model);
                foreach (var prop in entry.Properties)
                {
                    var propertyName = prop.Metadata.Name;
                    if (prop.Metadata.IsPrimaryKey())
                    {
                        model.KeyValues[propertyName] = prop.CurrentValue;
                        continue;
                    }
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            model.AuditType = AuditType.Create;
                            model.NewValues[propertyName] = prop.CurrentValue;
                            break;
                        case EntityState.Deleted:
                            model.AuditType = AuditType.Delete;
                            model.OldValues[propertyName] = prop.OriginalValue;
                            break;
                        case EntityState.Modified:
                            if (prop.IsModified)
                            {
                                model.AuditType = AuditType.Update;
                                model.NewValues[propertyName] = prop.CurrentValue;
                                model.OldValues[propertyName] = prop.OriginalValue;
                                model.ChangedColumns.Add(propertyName);
                            }
                            break;
                    }

                }
            }
            foreach (var auditEntry in auditDatas)
            {
                Audits.Add(auditEntry.ToAudit());
            }
        }
        
    }
}
