using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppWeb.Migrations
{
    /// <inheritdoc />
    public partial class AppPage_v1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppMenuTranslation_AppLanguages_AppLanguageId",
                table: "AppMenuTranslation");

            migrationBuilder.DropForeignKey(
                name: "FK_AppMenuTranslation_AppMenus_AppMenuId",
                table: "AppMenuTranslation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppMenuTranslation",
                table: "AppMenuTranslation");

            migrationBuilder.RenameTable(
                name: "AppMenuTranslation",
                newName: "AppMenuTranslations");

            migrationBuilder.RenameIndex(
                name: "IX_AppMenuTranslation_AppMenuId",
                table: "AppMenuTranslations",
                newName: "IX_AppMenuTranslations_AppMenuId");

            migrationBuilder.RenameIndex(
                name: "IX_AppMenuTranslation_AppLanguageId",
                table: "AppMenuTranslations",
                newName: "IX_AppMenuTranslations_AppLanguageId");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppMenuTranslations_AppLanguages_AppLanguageId",
                table: "AppMenuTranslations");

            migrationBuilder.DropForeignKey(
                name: "FK_AppMenuTranslations_AppMenus_AppMenuId",
                table: "AppMenuTranslations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppMenuTranslations",
                table: "AppMenuTranslations");

            migrationBuilder.RenameTable(
                name: "AppMenuTranslations",
                newName: "AppMenuTranslation");

            migrationBuilder.RenameIndex(
                name: "IX_AppMenuTranslations_AppMenuId",
                table: "AppMenuTranslation",
                newName: "IX_AppMenuTranslation_AppMenuId");

            migrationBuilder.RenameIndex(
                name: "IX_AppMenuTranslations_AppLanguageId",
                table: "AppMenuTranslation",
                newName: "IX_AppMenuTranslation_AppLanguageId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppMenuTranslation",
                table: "AppMenuTranslation",
                column: "Id");

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
        }
    }
}
