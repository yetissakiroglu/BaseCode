using Economy.Domain.Entites.TenantEntity.EntityAppSettings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Economy.Persistence.Tenant.Configurations.ConfigurationAppTechnicalSetting
{
  
    public class AppTechnicalSetting_Configuration : IEntityTypeConfiguration<AppTechnicalSetting>
    {
        public void Configure(EntityTypeBuilder<AppTechnicalSetting> entity)
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.DomainName)
                .HasMaxLength(255)
                .IsRequired();

            //entity.Property(x => x.StaticFileUrl)
            //    .HasMaxLength(500);

            entity.Property(x => x.MaintenanceMessage)
                .HasMaxLength(1000);

            //entity.Property(x => x.CustomCss)
            //    .HasColumnType("nvarchar(max)");

            //entity.Property(x => x.CustomJs)
            //    .HasColumnType("nvarchar(max)");

            //entity.Property(x => x.AllowedIpAddresses)
            //    .HasMaxLength(1000);

            //entity.Property(x => x.GoogleAnalyticsCode)
            //    .HasMaxLength(500);

            //entity.Property(x => x.FacebookPixelCode)
            //    .HasMaxLength(1000);
                    

            // Boolean alanlar (IsSiteLive, EnableCache vs.) için özel yapılandırmaya gerek yok

            // 🚀 Seed Data
            entity.HasData(new AppTechnicalSetting
            {
                Id = 1,
                //IsSiteLive = true,
                ForceSSL = true,
                DomainName = "www.otelsitem.com",
                //EnableCDN = false,
                //StaticFileUrl = "",
                EnableDebugMode = false,
                MaintenanceMessage = "Sitemiz şu anda bakım modundadır. Lütfen daha sonra tekrar deneyiniz.",
                EnableOutputCache = true,
                //CustomCss = "",
                //CustomJs = "",
                //EnableMaintenanceIpWhitelist = false,
                //AllowedIpAddresses = null,
                //GoogleAnalyticsCode = null,
                //FacebookPixelCode = null,
                IsDeleted = false
            });
        }
    }
}
