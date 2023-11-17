using DomainModule.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfrastructureModule.Mapping
{
    public class ActivityEntityMapping : IEntityTypeConfiguration<Activity>
    {
        public void Configure(EntityTypeBuilder<Activity> builder)
        {
            builder
              .HasKey(a => a.Id);

            builder
                  .Property(a => a.Id)
                  .HasColumnName("id");

            builder
                    .Property(a => a.UserId)
                    .HasColumnName("user_id");
            builder
                   .Property(a => a.Area)
                   .HasColumnName("area")
                   .HasMaxLength(100);

            builder
                  .Property(a => a.ControllerName)
                   .HasColumnName("controller")
                  .HasMaxLength(100);

            builder
                  .Property(a => a.IpAddress)
                   .HasColumnName("ip_address")
                  .HasMaxLength(200);


            builder
                  .Property(a => a.Browser)
                   .HasColumnName("browser")
                  .HasMaxLength(100);

            builder
                 .Property(a => a.ActionName)
                  .HasColumnName("action")
                  .HasMaxLength(100);

            builder
             .Property(a => a.SessionId)
              .HasColumnName("session_id")
              .HasMaxLength(100);

            builder
             .Property(a => a.UserName)
              .HasColumnName("user_name")
              .HasMaxLength(100);

            builder
              .Property(a => a.PageAccessed)
               .HasColumnName("page_accessed")
               .HasMaxLength(100);

            builder
              .Property(a => a.UrlReferrer)
               .HasColumnName("url_referer")
               .HasMaxLength(100);

            builder
                 .Property(a => a.ActionOn)
                  .HasColumnName("action_on")
                  .HasConversion<DateTime>();

            builder
                 .Property(a => a.Data)
                  .HasColumnName("data");
            builder
                .Property(a => a.Status)
                 .HasColumnName("status")
                 .HasMaxLength(100);
            builder
               .Property(a => a.QueryString)
                .HasColumnName("query_string");

            builder.ToTable("activity_logs");


        }
    }
}
