using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Economy.Base.Persistence.Migrations.HotelDb
{
    /// <inheritdoc />
    public partial class InitHotelDb : Migration
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
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsRTL = table.Column<bool>(type: "bit", nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    IsSiteLive = table.Column<bool>(type: "bit", nullable: false),
                    ForceSSL = table.Column<bool>(type: "bit", nullable: false),
                    DomainName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EnableCDN = table.Column<bool>(type: "bit", nullable: false),
                    StaticFileUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EnableDebugMode = table.Column<bool>(type: "bit", nullable: false),
                    CustomCss = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomJs = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AppVersion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaintenanceMessage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EnableCache = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppSettingTranslation",
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
                    table.PrimaryKey("PK_AppSettingTranslation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppSettingTranslation_AppLanguage_AppLanguageId",
                        column: x => x.AppLanguageId,
                        principalTable: "AppLanguage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppSettingTranslation_AppSettings_AppSettingId",
                        column: x => x.AppSettingId,
                        principalTable: "AppSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppSettingTranslation_AppLanguageId",
                table: "AppSettingTranslation",
                column: "AppLanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_AppSettingTranslation_AppSettingId",
                table: "AppSettingTranslation",
                column: "AppSettingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppSettingTranslation");

            migrationBuilder.DropTable(
                name: "AppLanguage");

            migrationBuilder.DropTable(
                name: "AppSettings");
        }
    }
}
