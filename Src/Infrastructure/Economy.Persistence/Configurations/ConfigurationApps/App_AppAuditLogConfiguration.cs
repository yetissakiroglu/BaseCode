using Economy.Domain.Entites.AppEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Persistence.Configurations.ConfigurationApps
{
    public class App_AppAuditLogConfiguration : IEntityTypeConfiguration<AppAuditLog>
    {
        public void Configure(EntityTypeBuilder<AppAuditLog> b)
        {
            b.ToTable("AppAuditLogs");
            b.HasKey(x => x.Id);

            b.Property(x => x.Action).IsRequired().HasMaxLength(64);
            b.Property(x => x.EntityName).HasMaxLength(128);
            b.Property(x => x.EntityId).HasMaxLength(128);
            b.Property(x => x.Message).HasMaxLength(512);
            b.Property(x => x.HttpMethod).HasMaxLength(16);
            b.Property(x => x.RequestPath).HasMaxLength(256);
            b.Property(x => x.IpAddress).HasMaxLength(64);
            b.Property(x => x.UserAgent).HasMaxLength(256);
            b.Property(x => x.CorrelationId).HasMaxLength(64);
            b.Property(x => x.UserName).HasMaxLength(256);

            // Sık arananlara index
            b.HasIndex(x => x.CreatedDate);
            b.HasIndex(x => x.UserId);
            b.HasIndex(x => new { x.Action, x.EntityName });
            b.HasIndex(x => x.CorrelationId);
            b.HasIndex(x => x.Succeeded);
        }
    }
}