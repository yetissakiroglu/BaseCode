using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Economy.Base.Persistence.Migrations.HotelDb
{
    /// <inheritdoc />
    public partial class InitHotelDbAppTechnicalSetting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppTechnicalSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsSiteLive = table.Column<bool>(type: "bit", nullable: false),
                    ForceSSL = table.Column<bool>(type: "bit", nullable: false),
                    DomainName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EnableCDN = table.Column<bool>(type: "bit", nullable: false),
                    StaticFileUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EnableDebugMode = table.Column<bool>(type: "bit", nullable: false),
                    AppVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaintenanceMessage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EnableCache = table.Column<bool>(type: "bit", nullable: false),
                    CustomCss = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomJs = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EnableMaintenanceIpWhitelist = table.Column<bool>(type: "bit", nullable: false),
                    AllowedIpAddresses = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EnableCustomHeaderScripts = table.Column<bool>(type: "bit", nullable: false),
                    EnableCustomFooterScripts = table.Column<bool>(type: "bit", nullable: false),
                    GoogleAnalyticsCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FacebookPixelCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EnableGlobalScriptInjection = table.Column<bool>(type: "bit", nullable: false),
                    EnablePreloader = table.Column<bool>(type: "bit", nullable: false),
                    PreloaderHtml = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppTechnicalSettings", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppTechnicalSettings");
        }
    }
}
