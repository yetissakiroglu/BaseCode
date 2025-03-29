using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppWeb.Migrations
{
    /// <inheritdoc />
    public partial class menu_v1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppMenus_AppMenus_ParentMenuId",
                table: "AppMenus");

            migrationBuilder.DropForeignKey(
                name: "FK_AppMenuTranslations_AppLanguages_AppLanguageId",
                table: "AppMenuTranslations");

            migrationBuilder.DropForeignKey(
                name: "FK_AppMenuTranslations_AppMenus_AppMenuId",
                table: "AppMenuTranslations");

            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "AppMenuTranslations",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "AppMenuTranslations",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "AppMenuId",
                table: "AppMenuTranslations",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AppMenus_AppMenus_ParentMenuId",
                table: "AppMenus",
                column: "ParentMenuId",
                principalTable: "AppMenus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AppMenuTranslations_AppLanguages_AppLanguageId",
                table: "AppMenuTranslations",
                column: "AppLanguageId",
                principalTable: "AppLanguages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AppMenuTranslations_AppMenus_AppMenuId",
                table: "AppMenuTranslations",
                column: "AppMenuId",
                principalTable: "AppMenus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppMenus_AppMenus_ParentMenuId",
                table: "AppMenus");

            migrationBuilder.DropForeignKey(
                name: "FK_AppMenuTranslations_AppLanguages_AppLanguageId",
                table: "AppMenuTranslations");

            migrationBuilder.DropForeignKey(
                name: "FK_AppMenuTranslations_AppMenus_AppMenuId",
                table: "AppMenuTranslations");

            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "AppMenuTranslations",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "AppMenuTranslations",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<int>(
                name: "AppMenuId",
                table: "AppMenuTranslations",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_AppMenus_AppMenus_ParentMenuId",
                table: "AppMenus",
                column: "ParentMenuId",
                principalTable: "AppMenus",
                principalColumn: "Id");

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
    }
}
