using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppWeb.Migrations
{
    /// <inheritdoc />
    public partial class AppContext_V11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppContentTranslation_AppContents_AppContentId",
                table: "AppContentTranslation");

            migrationBuilder.DropForeignKey(
                name: "FK_AppContentTranslation_AppLanguages_AppLanguageId",
                table: "AppContentTranslation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppContentTranslation",
                table: "AppContentTranslation");

            migrationBuilder.RenameTable(
                name: "AppContentTranslation",
                newName: "AppContentTranslations");

            migrationBuilder.RenameIndex(
                name: "IX_AppContentTranslation_AppLanguageId",
                table: "AppContentTranslations",
                newName: "IX_AppContentTranslations_AppLanguageId");

            migrationBuilder.RenameIndex(
                name: "IX_AppContentTranslation_AppContentId",
                table: "AppContentTranslations",
                newName: "IX_AppContentTranslations_AppContentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppContentTranslations",
                table: "AppContentTranslations",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppContentTranslations_AppContents_AppContentId",
                table: "AppContentTranslations",
                column: "AppContentId",
                principalTable: "AppContents",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppContentTranslations_AppLanguages_AppLanguageId",
                table: "AppContentTranslations",
                column: "AppLanguageId",
                principalTable: "AppLanguages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppContentTranslations_AppContents_AppContentId",
                table: "AppContentTranslations");

            migrationBuilder.DropForeignKey(
                name: "FK_AppContentTranslations_AppLanguages_AppLanguageId",
                table: "AppContentTranslations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppContentTranslations",
                table: "AppContentTranslations");

            migrationBuilder.RenameTable(
                name: "AppContentTranslations",
                newName: "AppContentTranslation");

            migrationBuilder.RenameIndex(
                name: "IX_AppContentTranslations_AppLanguageId",
                table: "AppContentTranslation",
                newName: "IX_AppContentTranslation_AppLanguageId");

            migrationBuilder.RenameIndex(
                name: "IX_AppContentTranslations_AppContentId",
                table: "AppContentTranslation",
                newName: "IX_AppContentTranslation_AppContentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppContentTranslation",
                table: "AppContentTranslation",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppContentTranslation_AppContents_AppContentId",
                table: "AppContentTranslation",
                column: "AppContentId",
                principalTable: "AppContents",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppContentTranslation_AppLanguages_AppLanguageId",
                table: "AppContentTranslation",
                column: "AppLanguageId",
                principalTable: "AppLanguages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
