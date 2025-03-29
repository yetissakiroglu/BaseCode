using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppWeb.Migrations
{
    /// <inheritdoc />
    public partial class language_v3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "UserRefreshToken");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "UserRefreshToken");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "UserRefreshToken");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "UserRefreshToken");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "AppTechnicalSettings");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "AppTechnicalSettings");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "AppTechnicalSettings");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "AppTechnicalSettings");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "AppSlideTranslations");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "AppSlideTranslations");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "AppSlideTranslations");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "AppSlideTranslations");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "AppSlides");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "AppSlides");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "AppSlides");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "AppSlides");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "AppSettingTranslations");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "AppSettingTranslations");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "AppSettingTranslations");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "AppSettingTranslations");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "AppSettings");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "AppSections");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "AppSections");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "AppSections");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "AppSections");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "AppPageTranslations");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "AppPageTranslations");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "AppPageTranslations");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "AppPageTranslations");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "AppPageSections");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "AppPageSections");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "AppPageSections");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "AppPageSections");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "AppPages");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "AppPages");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "AppPages");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "AppPages");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "AppMenuTranslations");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "AppMenuTranslations");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "AppMenuTranslations");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "AppMenuTranslations");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "AppMenus");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "AppMenus");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "AppMenus");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "AppMenus");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "AppLanguages");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "AppLanguages");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "AppLanguages");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "AppLanguages");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "AppContentTranslations");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "AppContentTranslations");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "AppContentTranslations");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "AppContentTranslations");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "AppContents");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "AppContents");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "AppContents");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "AppContents");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "AppCategories");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "AppCategories");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "AppCategories");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "AppCategories");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "UserRefreshToken",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "UserRefreshToken",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "UserRefreshToken",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "UserRefreshToken",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "AppTechnicalSettings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "AppTechnicalSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "AppTechnicalSettings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "AppTechnicalSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "AppSlideTranslations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "AppSlideTranslations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "AppSlideTranslations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "AppSlideTranslations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "AppSlides",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "AppSlides",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "AppSlides",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "AppSlides",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "AppSettingTranslations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "AppSettingTranslations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "AppSettingTranslations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "AppSettingTranslations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "AppSettings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "AppSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "AppSettings",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "AppSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "AppSections",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "AppSections",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "AppSections",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "AppSections",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "AppPageTranslations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "AppPageTranslations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "AppPageTranslations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "AppPageTranslations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "AppPageSections",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "AppPageSections",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "AppPageSections",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "AppPageSections",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "AppPages",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "AppPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "AppPages",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "AppPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "AppMenuTranslations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "AppMenuTranslations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "AppMenuTranslations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "AppMenuTranslations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "AppMenus",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "AppMenus",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "AppMenus",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "AppMenus",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "AppLanguages",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "AppLanguages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "AppLanguages",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "AppLanguages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "AppContentTranslations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "AppContentTranslations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "AppContentTranslations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "AppContentTranslations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "AppContents",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "AppContents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "AppContents",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "AppContents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "AppCategories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "AppCategories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "AppCategories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "AppCategories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AppLanguages",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "CreatedBy", "UpdatedAt", "UpdatedBy" },
                values: new object[] { new DateTime(2025, 3, 29, 6, 13, 45, 526, DateTimeKind.Local).AddTicks(6676), "", null, "" });

            migrationBuilder.UpdateData(
                table: "AppLanguages",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "CreatedBy", "UpdatedAt", "UpdatedBy" },
                values: new object[] { new DateTime(2025, 3, 29, 6, 13, 45, 528, DateTimeKind.Local).AddTicks(1232), "", null, "" });

            migrationBuilder.UpdateData(
                table: "AppLanguages",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "CreatedBy", "UpdatedAt", "UpdatedBy" },
                values: new object[] { new DateTime(2025, 3, 29, 6, 13, 45, 528, DateTimeKind.Local).AddTicks(1242), "", null, "" });

            migrationBuilder.UpdateData(
                table: "AppTechnicalSettings",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "CreatedBy", "UpdatedAt", "UpdatedBy" },
                values: new object[] { new DateTime(2025, 3, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "1", new DateTime(2025, 3, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "1" });
        }
    }
}
