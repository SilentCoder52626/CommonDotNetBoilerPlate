using DomainModule.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;


namespace InfrastructureModule.Mapping
{
    public class UserEntityMapping : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(a => a.Name).HasMaxLength(100).IsRequired();
            builder.Property(a => a.CreatedOn).IsRequired();
            builder.Property(a => a.Status).HasMaxLength(100).IsRequired();
            builder.Property(a => a.Type).HasMaxLength(100).IsRequired();
            builder.ToTable("AspNetUsers");

        }
    }
}
