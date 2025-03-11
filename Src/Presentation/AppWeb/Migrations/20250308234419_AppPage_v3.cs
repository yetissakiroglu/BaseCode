using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppWeb.Migrations
{
    /// <inheritdoc />
    public partial class AppPage_v3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppMenuTranslations_AppLanguages_AppLanguageId",
                table: "AppMenuTranslations");

            migrationBuilder.DropForeignKey(
                name: "FK_AppMenuTranslations_AppMenus_AppMenuId",
                table: "AppMenuTranslations");

            migrationBuilder.DropForeignKey(
                name: "FK_AppSettingTranslations_AppLanguages_AppLanguageId",
                table: "AppSettingTranslations");

            migrationBuilder.DropForeignKey(
                name: "FK_AppSettingTranslations_AppSettings_AppSettingId",
                table: "AppSettingTranslations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppSettingTranslations",
                table: "AppSettingTranslations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppMenuTranslations",
                table: "AppMenuTranslations");

            migrationBuilder.RenameTable(
                name: "AppSettingTranslations",
                newName: "AppSettingTranslation");

            migrationBuilder.RenameTable(
                name: "AppMenuTranslations",
                newName: "AppMenuTranslation");

            migrationBuilder.RenameIndex(
                name: "IX_AppSettingTranslations_AppSettingId",
                table: "AppSettingTranslation",
                newName: "IX_AppSettingTranslation_AppSettingId");

            migrationBuilder.RenameIndex(
                name: "IX_AppSettingTranslations_AppLanguageId",
                table: "AppSettingTranslation",
                newName: "IX_AppSettingTranslation_AppLanguageId");

            migrationBuilder.RenameIndex(
                name: "IX_AppMenuTranslations_AppMenuId",
                table: "AppMenuTranslation",
                newName: "IX_AppMenuTranslation_AppMenuId");

            migrationBuilder.RenameIndex(
                name: "IX_AppMenuTranslations_AppLanguageId",
                table: "AppMenuTranslation",
                newName: "IX_AppMenuTranslation_AppLanguageId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppSettingTranslation",
                table: "AppSettingTranslation",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppMenuTranslation",
                table: "AppMenuTranslation",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "AppPages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsHomePage = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppPages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppPageTranslation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppLanguageId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShortDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MetaTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MetaDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AppPageId = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppPageTranslation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppPageTranslation_AppLanguages_AppLanguageId",
                        column: x => x.AppLanguageId,
                        principalTable: "AppLanguages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppPageTranslation_AppPages_AppPageId",
                        column: x => x.AppPageId,
                        principalTable: "AppPages",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppPageTranslation_AppLanguageId",
                table: "AppPageTranslation",
                column: "AppLanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_AppPageTranslation_AppPageId",
                table: "AppPageTranslation",
                column: "AppPageId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppMenuTranslation_AppLanguages_AppLanguageId",
                table: "AppMenuTranslation",
                column: "AppLanguageId",
                principalTable: "AppLanguages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppMenuTranslation_AppMenus_AppMenuId",
                table: "AppMenuTranslation",
                column: "AppMenuId",
                principalTable: "AppMenus",
                principalColumn: "Id");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppMenuTranslation_AppLanguages_AppLanguageId",
                table: "AppMenuTranslation");

            migrationBuilder.DropForeignKey(
                name: "FK_AppMenuTranslation_AppMenus_AppMenuId",
                table: "AppMenuTranslation");

            migrationBuilder.DropForeignKey(
                name: "FK_AppSettingTranslation_AppLanguages_AppLanguageId",
                table: "AppSettingTranslation");

            migrationBuilder.DropForeignKey(
                name: "FK_AppSettingTranslation_AppSettings_AppSettingId",
                table: "AppSettingTranslation");

            migrationBuilder.DropTable(
                name: "AppPageTranslation");

            migrationBuilder.DropTable(
                name: "AppPages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppSettingTranslation",
                table: "AppSettingTranslation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppMenuTranslation",
                table: "AppMenuTranslation");

            migrationBuilder.RenameTable(
                name: "AppSettingTranslation",
                newName: "AppSettingTranslations");

            migrationBuilder.RenameTable(
                name: "AppMenuTranslation",
                newName: "AppMenuTranslations");

            migrationBuilder.RenameIndex(
                name: "IX_AppSettingTranslation_AppSettingId",
                table: "AppSettingTranslations",
                newName: "IX_AppSettingTranslations_AppSettingId");

            migrationBuilder.RenameIndex(
                name: "IX_AppSettingTranslation_AppLanguageId",
                table: "AppSettingTranslations",
                newName: "IX_AppSettingTranslations_AppLanguageId");

            migrationBuilder.RenameIndex(
                name: "IX_AppMenuTranslation_AppMenuId",
                table: "AppMenuTranslations",
                newName: "IX_AppMenuTranslations_AppMenuId");

            migrationBuilder.RenameIndex(
                name: "IX_AppMenuTranslation_AppLanguageId",
                table: "AppMenuTranslations",
                newName: "IX_AppMenuTranslations_AppLanguageId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppSettingTranslations",
                table: "AppSettingTranslations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppMenuTranslations",
                table: "AppMenuTranslations",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppMenuTranslations_AppLanguages_AppLanguageId",
                table: "AppMenuTranslations",
                column: "AppLanguageId",
                principalTable: "AppLanguages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppMenuTranslations_AppMenus_AppMenuId",
                table: "AppMenuTranslations",
                column: "AppMenuId",
                principalTable: "AppMenus",
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
    }
}
