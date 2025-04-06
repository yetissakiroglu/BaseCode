using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AppWeb.Migrations
{
    /// <inheritdoc />
    public partial class page_v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppPageTranslations_AppPages_AppPageId",
                table: "AppPageTranslations");

            migrationBuilder.AlterColumn<int>(
                name: "AppPageId",
                table: "AppPageTranslations",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AppPages",
                columns: new[] { "Id", "IsDeleted", "IsHomePage" },
                values: new object[] { 1, false, true });

            migrationBuilder.InsertData(
                table: "AppPageTranslations",
                columns: new[] { "Id", "AppLanguageId", "AppPageId", "Content", "IsDeleted", "MetaDescription", "MetaTitle", "Title", "Url" },
                values: new object[,]
                {
                    { 1, 1, 1, "Anasayfa", false, "Anasayfa", "Anasayfa", "Anasayfa", "anasayfa" },
                    { 2, 2, 1, "Home", false, "Home", "Home", "Home", "home" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_AppPageTranslations_AppPages_AppPageId",
                table: "AppPageTranslations",
                column: "AppPageId",
                principalTable: "AppPages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppPageTranslations_AppPages_AppPageId",
                table: "AppPageTranslations");

            migrationBuilder.DeleteData(
                table: "AppPageTranslations",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AppPageTranslations",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AppPages",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.AlterColumn<int>(
                name: "AppPageId",
                table: "AppPageTranslations",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_AppPageTranslations_AppPages_AppPageId",
                table: "AppPageTranslations",
                column: "AppPageId",
                principalTable: "AppPages",
                principalColumn: "Id");
        }
    }
}
