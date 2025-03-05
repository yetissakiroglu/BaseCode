using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppWeb.Migrations
{
    /// <inheritdoc />
    public partial class AppSetting_Language_Create : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IconCode",
                table: "AppLanguages");

            migrationBuilder.DropColumn(
                name: "ResourceFileName",
                table: "AppLanguages");

            migrationBuilder.RenameColumn(
                name: "LanguageCode",
                table: "AppLanguages",
                newName: "Icon");

            migrationBuilder.RenameColumn(
                name: "IsEnabled",
                table: "AppLanguages",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "CultureInfo",
                table: "AppLanguages",
                newName: "Code");

            migrationBuilder.CreateTable(
                name: "AppSettingTranslation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppSettingId = table.Column<int>(type: "int", nullable: false),
                    AppLanguageId = table.Column<int>(type: "int", nullable: false),
                    TranslatedSiteTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TranslatedSiteDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TranslatedMetaTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TranslatedMetaDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSettingTranslation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppSettingTranslation_AppLanguages_AppLanguageId",
                        column: x => x.AppLanguageId,
                        principalTable: "AppLanguages",
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

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "AppLanguages",
                newName: "IsEnabled");

            migrationBuilder.RenameColumn(
                name: "Icon",
                table: "AppLanguages",
                newName: "LanguageCode");

            migrationBuilder.RenameColumn(
                name: "Code",
                table: "AppLanguages",
                newName: "CultureInfo");

            migrationBuilder.AddColumn<string>(
                name: "IconCode",
                table: "AppLanguages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResourceFileName",
                table: "AppLanguages",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
