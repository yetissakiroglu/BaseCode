using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Economy.Base.Persistence.Migrations.HotelDb
{
    /// <inheritdoc />
    public partial class InitHotelDb1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppLanguage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsRTL = table.Column<bool>(type: "bit", nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppLanguage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSettings", x => x.Id);
                });

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

            migrationBuilder.CreateTable(
                name: "AppSettingTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppSettingId = table.Column<int>(type: "int", nullable: false),
                    AppLanguageId = table.Column<int>(type: "int", nullable: false),
                    SiteTitle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    MetaTitle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    MetaDescription = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSettingTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppSettingTranslations_AppSettings_AppSettingId",
                        column: x => x.AppSettingId,
                        principalTable: "AppSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AppLanguage",
                columns: new[] { "Id", "Code", "Icon", "IsActive", "IsDefault", "IsDeleted", "IsRTL", "Name" },
                values: new object[,]
                {
                    { 1, "tr", "flag-icon flag-icon-tur", true, true, false, false, "Türkçe" },
                    { 2, "en", "flag-icon flag-icon-gbr", true, false, false, false, "English" },
                    { 3, "ar", "flag-icon flag-icon-sau", true, false, false, true, "العربية" }
                });

            migrationBuilder.InsertData(
                table: "AppSettings",
                columns: new[] { "Id", "IsDeleted" },
                values: new object[] { 1, false });

            migrationBuilder.InsertData(
                table: "AppSettingTranslations",
                columns: new[] { "Id", "AppLanguageId", "AppSettingId", "Description", "IsDeleted", "MetaDescription", "MetaTitle", "SiteTitle" },
                values: new object[,]
                {
                    { 1, 1, 1, "Site açıklaması Türkçe", false, "Meta açıklaması Türkçe", "Meta Başlık - TR", "Site Başlığı - TR" },
                    { 2, 2, 1, "Site description in English", false, "Meta description in English", "Meta Title - EN", "Site Title - EN" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppSettingTranslations_AppSettingId_AppLanguageId",
                table: "AppSettingTranslations",
                columns: new[] { "AppSettingId", "AppLanguageId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppLanguage");

            migrationBuilder.DropTable(
                name: "AppSettingTranslations");

            migrationBuilder.DropTable(
                name: "AppTechnicalSettings");

            migrationBuilder.DropTable(
                name: "AppSettings");
        }
    }
}
