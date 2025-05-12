using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Economy.Base.Persistence.Migrations.HotelDb
{
    /// <inheritdoc />
    public partial class InitHotelDbSetting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppSettingTranslation_AppLanguage_AppLanguageId",
                table: "AppSettingTranslation");

            migrationBuilder.DropForeignKey(
                name: "FK_AppSettingTranslation_AppSettings_AppSettingId",
                table: "AppSettingTranslation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppSettingTranslation",
                table: "AppSettingTranslation");

            migrationBuilder.DropIndex(
                name: "IX_AppSettingTranslation_AppLanguageId",
                table: "AppSettingTranslation");

            migrationBuilder.DropIndex(
                name: "IX_AppSettingTranslation_AppSettingId",
                table: "AppSettingTranslation");

            migrationBuilder.DropColumn(
                name: "AppVersion",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "CustomCss",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "CustomJs",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "DomainName",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "EnableCDN",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "EnableCache",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "EnableDebugMode",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "ForceSSL",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "IsSiteLive",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "MaintenanceMessage",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "StaticFileUrl",
                table: "AppSettings");

            migrationBuilder.RenameTable(
                name: "AppSettingTranslation",
                newName: "AppSettingTranslations");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppSettingTranslations",
                table: "AppSettingTranslations",
                column: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_AppSettingTranslations_AppSettings_AppSettingId",
                table: "AppSettingTranslations",
                column: "AppSettingId",
                principalTable: "AppSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppSettingTranslations_AppSettings_AppSettingId",
                table: "AppSettingTranslations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppSettingTranslations",
                table: "AppSettingTranslations");

            migrationBuilder.DropIndex(
                name: "IX_AppSettingTranslations_AppSettingId_AppLanguageId",
                table: "AppSettingTranslations");

            migrationBuilder.DeleteData(
                table: "AppSettingTranslations",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AppSettingTranslations",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AppSettings",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.RenameTable(
                name: "AppSettingTranslations",
                newName: "AppSettingTranslation");

            migrationBuilder.AddColumn<string>(
                name: "AppVersion",
                table: "AppSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CustomCss",
                table: "AppSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CustomJs",
                table: "AppSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DomainName",
                table: "AppSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "EnableCDN",
                table: "AppSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EnableCache",
                table: "AppSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "EnableDebugMode",
                table: "AppSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ForceSSL",
                table: "AppSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSiteLive",
                table: "AppSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "MaintenanceMessage",
                table: "AppSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StaticFileUrl",
                table: "AppSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppSettingTranslation",
                table: "AppSettingTranslation",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_AppSettingTranslation_AppLanguageId",
                table: "AppSettingTranslation",
                column: "AppLanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_AppSettingTranslation_AppSettingId",
                table: "AppSettingTranslation",
                column: "AppSettingId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppSettingTranslation_AppLanguage_AppLanguageId",
                table: "AppSettingTranslation",
                column: "AppLanguageId",
                principalTable: "AppLanguage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppSettingTranslation_AppSettings_AppSettingId",
                table: "AppSettingTranslation",
                column: "AppSettingId",
                principalTable: "AppSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
