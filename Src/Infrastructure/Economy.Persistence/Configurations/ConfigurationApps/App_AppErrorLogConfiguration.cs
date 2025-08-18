using Economy.Domain.Entites.AppEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Economy.Persistence.Configurations.ConfigurationApps
{
    public class App_AppErrorLogConfiguration : IEntityTypeConfiguration<AppErrorLog>
    {
        public void Configure(EntityTypeBuilder<AppErrorLog> b)
        {
            b.ToTable("AppErrorLogs");
            b.HasKey(x => x.Id);

                b.Property(x => x.HttpMethod).HasMaxLength(16);
            b.Property(x => x.RequestPath).HasMaxLength(256);
            b.Property(x => x.QueryString).HasMaxLength(512);
            b.Property(x => x.IpAddress).HasMaxLength(64);
            b.Property(x => x.UserAgent).HasMaxLength(256);
            b.Property(x => x.CorrelationId).HasMaxLength(64);
            b.Property(x => x.UserName).HasMaxLength(256);

            b.Property(x => x.ExceptionType).HasMaxLength(256);
            b.Property(x => x.Message).HasMaxLength(1024);
            b.Property(x => x.Source).HasMaxLength(256);
            b.Property(x => x.TargetSite).HasMaxLength(256);

            // Indexler
            b.HasIndex(x => x.CreatedAt);
            b.HasIndex(x => x.StatusCode);
            b.HasIndex(x => x.CorrelationId);
            b.HasIndex(x => x.UserId);
        }
    }
}