using Economy.Domain.Entites.EntityAppSettings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Economy.Persistence.Configurations.ConfigurationAppTechnicalSetting
{
    public static class SeedIds
    {
        public static readonly Guid TechnicalSettingId = new Guid("B56A8EE5-5B38-4F2C-9EE8-0BFD8CC0C42B");
    }

    public class AppTechnicalSetting_Configuration : IEntityTypeConfiguration<AppTechnicalSetting>
    {
        public void Configure(EntityTypeBuilder<AppTechnicalSetting> entity)
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.DomainName)
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(x => x.StaticFileUrl)
                .HasMaxLength(500);

            entity.Property(x => x.AppVersion)
                .HasMaxLength(50);

            entity.Property(x => x.MaintenanceMessage)
                .HasMaxLength(1000);

            entity.Property(x => x.CustomCss)
                .HasColumnType("nvarchar(max)");

            entity.Property(x => x.CustomJs)
                .HasColumnType("nvarchar(max)");

            entity.Property(x => x.AllowedIpAddresses)
                .HasMaxLength(1000);

            entity.Property(x => x.GoogleAnalyticsCode)
                .HasMaxLength(500);

            entity.Property(x => x.FacebookPixelCode)
                .HasMaxLength(1000);

            entity.Property(x => x.PreloaderHtml)
                .HasColumnType("nvarchar(max)");

            // Boolean alanlar (IsSiteLive, EnableCache vs.) için özel yapılandırmaya gerek yok

            // 🚀 Seed Data
            entity.HasData(new AppTechnicalSetting
            {
                Id = 1,
                IsSiteLive = true,
                ForceSSL = true,
                DomainName = "www.otelsitem.com",
                EnableCDN = false,
                StaticFileUrl = "",
                EnableDebugMode = false,
                AppVersion = "v1.0.0",
                MaintenanceMessage = "Sitemiz şu anda bakım modundadır. Lütfen daha sonra tekrar deneyiniz.",
                EnableCache = true,
                CustomCss = "",
                CustomJs = "",
                EnableMaintenanceIpWhitelist = false,
                AllowedIpAddresses = null,
                EnableCustomHeaderScripts = false,
                EnableCustomFooterScripts = false,
                GoogleAnalyticsCode = null,
                FacebookPixelCode = null,
                EnableGlobalScriptInjection = false,
                EnablePreloader = false,
                PreloaderHtml = null,
                IsDeleted = false,
                CreatedAt = new DateTime(2025, 03, 28, 0, 0, 0),  // ✅ SABİT DEĞER
                CreatedBy = "1",
                UpdatedAt = new DateTime(2025, 03, 28, 0, 0, 0),  // ✅ SABİT DEĞER
                UpdatedBy = "1"

            });
        }
    }
}
