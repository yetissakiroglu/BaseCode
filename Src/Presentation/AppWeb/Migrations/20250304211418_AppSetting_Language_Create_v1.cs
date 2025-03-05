using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppWeb.Migrations
{
    /// <inheritdoc />
    public partial class AppSetting_Language_Create_v1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppCategories_AppCategories_ParentCategoryId",
                table: "AppCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_AppSettingTranslation_AppLanguages_AppLanguageId",
                table: "AppSettingTranslation");

            migrationBuilder.DropForeignKey(
                name: "FK_AppSettingTranslation_AppSettings_AppSettingId",
                table: "AppSettingTranslation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppSettingTranslation",
                table: "AppSettingTranslation");

            migrationBuilder.DropColumn(
                name: "MaintenanceMessage",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "SiteFavicon",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "SiteLogo",
                table: "AppSettings");

            migrationBuilder.RenameTable(
                name: "AppSettingTranslation",
                newName: "AppSettingTranslations");

            migrationBuilder.RenameColumn(
                name: "TimeZone",
                table: "AppSettings",
                newName: "SiteTitle");

            migrationBuilder.RenameColumn(
                name: "SiteName",
                table: "AppSettings",
                newName: "SiteDescription");

            migrationBuilder.RenameColumn(
                name: "IsMaintenanceMode",
                table: "AppSettings",
                newName: "IsActive");

            migrationBuilder.RenameIndex(
                name: "IX_AppSettingTranslation_AppSettingId",
                table: "AppSettingTranslations",
                newName: "IX_AppSettingTranslations_AppSettingId");

            migrationBuilder.RenameIndex(
                name: "IX_AppSettingTranslation_AppLanguageId",
                table: "AppSettingTranslations",
                newName: "IX_AppSettingTranslations_AppLanguageId");

            migrationBuilder.AlterColumn<string>(
                name: "MetaTitle",
                table: "AppSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MetaDescription",
                table: "AppSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactEmail",
                table: "AppSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContactPhone",
                table: "AppSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LogoUrl",
                table: "AppSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PrimaryColor",
                table: "AppSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Slug",
                table: "AppCategories",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AppCategories",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "MetaTitle",
                table: "AppCategories",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MetaDescription",
                table: "AppCategories",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppSettingTranslations",
                table: "AppSettingTranslations",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppCategories_AppCategories_ParentCategoryId",
                table: "AppCategories",
                column: "ParentCategoryId",
                principalTable: "AppCategories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppSettingTranslations_AppLanguages_AppLanguageId",
                table: "AppSettingTranslations",
                column: "AppLanguageId",
                principalTable: "AppLanguages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
                name: "FK_AppCategories_AppCategories_ParentCategoryId",
                table: "AppCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_AppSettingTranslations_AppLanguages_AppLanguageId",
                table: "AppSettingTranslations");

            migrationBuilder.DropForeignKey(
                name: "FK_AppSettingTranslations_AppSettings_AppSettingId",
                table: "AppSettingTranslations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppSettingTranslations",
                table: "AppSettingTranslations");

            migrationBuilder.DropColumn(
                name: "ContactEmail",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "ContactPhone",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "LogoUrl",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "PrimaryColor",
                table: "AppSettings");

            migrationBuilder.RenameTable(
                name: "AppSettingTranslations",
                newName: "AppSettingTranslation");

            migrationBuilder.RenameColumn(
                name: "SiteTitle",
                table: "AppSettings",
                newName: "TimeZone");

            migrationBuilder.RenameColumn(
                name: "SiteDescription",
                table: "AppSettings",
                newName: "SiteName");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "AppSettings",
                newName: "IsMaintenanceMode");

            migrationBuilder.RenameIndex(
                name: "IX_AppSettingTranslations_AppSettingId",
                table: "AppSettingTranslation",
                newName: "IX_AppSettingTranslation_AppSettingId");

            migrationBuilder.RenameIndex(
                name: "IX_AppSettingTranslations_AppLanguageId",
                table: "AppSettingTranslation",
                newName: "IX_AppSettingTranslation_AppLanguageId");

            migrationBuilder.AlterColumn<string>(
                name: "MetaTitle",
                table: "AppSettings",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "MetaDescription",
                table: "AppSettings",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "MaintenanceMessage",
                table: "AppSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SiteFavicon",
                table: "AppSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SiteLogo",
                table: "AppSettings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Slug",
                table: "AppCategories",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AppCategories",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "MetaTitle",
                table: "AppCategories",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MetaDescription",
                table: "AppCategories",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppSettingTranslation",
                table: "AppSettingTranslation",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppCategories_AppCategories_ParentCategoryId",
                table: "AppCategories",
                column: "ParentCategoryId",
                principalTable: "AppCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AppSettingTranslation_AppLanguages_AppLanguageId",
                table: "AppSettingTranslation",
                column: "AppLanguageId",
                principalTable: "AppLanguages",
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
