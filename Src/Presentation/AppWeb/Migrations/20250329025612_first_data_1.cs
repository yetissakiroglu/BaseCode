using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppWeb.Migrations
{
    /// <inheritdoc />
    public partial class first_data_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AppTechnicalSettings",
                keyColumn: "Id",
                keyValue: -1);

            migrationBuilder.InsertData(
                table: "AppTechnicalSettings",
                columns: new[] { "Id", "AllowedIpAddresses", "AppVersion", "CreatedAt", "CreatedBy", "CustomCss", "CustomJs", "DomainName", "EnableCDN", "EnableCache", "EnableCustomFooterScripts", "EnableCustomHeaderScripts", "EnableDebugMode", "EnableGlobalScriptInjection", "EnableMaintenanceIpWhitelist", "EnablePreloader", "FacebookPixelCode", "ForceSSL", "GoogleAnalyticsCode", "IsDeleted", "IsSiteLive", "MaintenanceMessage", "PreloaderHtml", "StaticFileUrl", "UpdatedAt", "UpdatedBy" },
                values: new object[] { 1, null, "v1.0.0", new DateTime(2025, 3, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "1", "", "", "www.otelsitem.com", false, true, false, false, false, false, false, false, null, true, null, false, true, "Sitemiz şu anda bakım modundadır. Lütfen daha sonra tekrar deneyiniz.", null, "", new DateTime(2025, 3, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "1" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AppTechnicalSettings",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.InsertData(
                table: "AppTechnicalSettings",
                columns: new[] { "Id", "AllowedIpAddresses", "AppVersion", "CreatedAt", "CreatedBy", "CustomCss", "CustomJs", "DomainName", "EnableCDN", "EnableCache", "EnableCustomFooterScripts", "EnableCustomHeaderScripts", "EnableDebugMode", "EnableGlobalScriptInjection", "EnableMaintenanceIpWhitelist", "EnablePreloader", "FacebookPixelCode", "ForceSSL", "GoogleAnalyticsCode", "IsDeleted", "IsSiteLive", "MaintenanceMessage", "PreloaderHtml", "StaticFileUrl", "UpdatedAt", "UpdatedBy" },
                values: new object[] { -1, null, "v1.0.0", new DateTime(2025, 3, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "1", "", "", "www.otelsitem.com", false, true, false, false, false, false, false, false, null, true, null, false, true, "Sitemiz şu anda bakım modundadır. Lütfen daha sonra tekrar deneyiniz.", null, "", new DateTime(2025, 3, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "1" });
        }
    }
}
