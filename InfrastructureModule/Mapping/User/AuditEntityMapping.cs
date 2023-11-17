using DomainModule.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;


namespace InfrastructureModule.Mapping
{
    public class AuditEntityMapping : IEntityTypeConfiguration<Audit>
    {
        public void Configure(EntityTypeBuilder<Audit> builder)
        {
            builder
                .HasKey(a => a.Id);
            builder
                    .Property(a => a.UserId)
                    .HasColumnName("user_id");
            builder
                   .Property(a => a.Type)
                   .HasColumnName("type")
                   .IsRequired();
            builder
                  .Property(a => a.TableName)
                   .HasColumnName("table_name")
                  .HasMaxLength(100)
                  .IsRequired();
            builder
                  .Property(a => a.IpAddress)
                   .HasColumnName("ip_address")
                  .HasMaxLength(200)
                  .IsRequired();
            builder
                  .Property(a => a.Browser)
                   .HasColumnName("browser")
                  .HasMaxLength(100);
            builder
                .Property(a => a.DateTime)
                .HasColumnName("action_on")
                .HasDefaultValue(DateTime.Now)
                .IsRequired();
            builder
                .Property(a => a.OldValues)
                .HasColumnName("old_values");
            builder
               .Property(a => a.NewValues)
               .HasColumnName("new_values");
            builder
                .Property(a => a.PrimaryKey)
                .HasColumnName("keys");
            builder
                .ToTable("audit_logs");
        }

    }
}
