using DomainModule.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfrastructureModule.Mapping
{
    public class AppSettingsEntityMapping : IEntityTypeConfiguration<AppSettings>
    {
        public void Configure(EntityTypeBuilder<AppSettings> builder)
        {
            builder
              .HasKey(a => a.Id);

            builder
                  .Property(a => a.Id)
                  .HasColumnName("id");

            builder
                    .Property(a => a.UserId)
                    .HasColumnName("user_id")
                    .IsRequired();
            builder
                   .Property(a => a.Key)
                   .HasColumnName("key")
                   .HasMaxLength(500)
                   .IsRequired();
            builder
                   .Property(a => a.Value)
                   .HasColumnName("value")
                   .HasMaxLength(500)
                   .IsRequired();

            builder
                .HasIndex(a => new { a.Key, a.UserId })
                .IsUnique();

            builder.ToTable("appsettings");


        }
    }
}
