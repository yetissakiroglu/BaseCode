using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AppWeb.Migrations
{
    /// <inheritdoc />
    public partial class page_v7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppSlideTranslations_AppSlides_AppSlideId",
                table: "AppSlideTranslations");

            migrationBuilder.DropColumn(
                name: "Thumbnail",
                table: "AppSlideTranslations");

            migrationBuilder.AlterColumn<int>(
                name: "AppSlideId",
                table: "AppSlideTranslations",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Thumbnail",
                table: "AppSlides",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AppSlides",
                columns: new[] { "Id", "AppSectionId", "IsDeleted", "Sequence", "Thumbnail" },
                values: new object[,]
                {
                    { 1, 1, false, 1, "slide_1.jpg" },
                    { 2, 1, false, 1, "slide_2.jpg" },
                    { 3, 1, false, 1, "slide_3.jpg" }
                });

            migrationBuilder.InsertData(
                table: "AppSlideTranslations",
                columns: new[] { "Id", "AppLanguageId", "AppSlideId", "ButtonIcon", "ButtonText", "ButtonUrl", "Content", "IsDeleted", "IsExternal", "Title" },
                values: new object[,]
                {
                    { 1, 1, 1, "fa-search", "Keşfet", "/explore", "En iyi tatil deneyimi için bizimle olun.", false, false, "Hoş Geldiniz" },
                    { 2, 2, 1, "fa-search", "Explore", "/explore", "Join us for the best vacation experience.", false, false, "Welcome" },
                    { 3, 1, 2, "fa-search", "Keşfet", "/explore", "En iyi tatil deneyimi için bizimle olun.", false, false, "Hoş Geldiniz" },
                    { 4, 2, 2, "fa-search", "Explore", "/explore", "Join us for the best vacation experience.", false, false, "Welcome" },
                    { 5, 1, 3, "fa-search", "Keşfet", "/explore", "En iyi tatil deneyimi için bizimle olun.", false, false, "Hoş Geldiniz" },
                    { 6, 2, 3, "fa-search", "Explore", "/explore", "Join us for the best vacation experience.", false, false, "Welcome" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_AppSlideTranslations_AppSlides_AppSlideId",
                table: "AppSlideTranslations",
                column: "AppSlideId",
                principalTable: "AppSlides",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppSlideTranslations_AppSlides_AppSlideId",
                table: "AppSlideTranslations");

            migrationBuilder.DeleteData(
                table: "AppSlideTranslations",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AppSlideTranslations",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AppSlideTranslations",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AppSlideTranslations",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "AppSlideTranslations",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "AppSlideTranslations",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "AppSlides",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AppSlides",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AppSlides",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DropColumn(
                name: "Thumbnail",
                table: "AppSlides");

            migrationBuilder.AlterColumn<int>(
                name: "AppSlideId",
                table: "AppSlideTranslations",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Thumbnail",
                table: "AppSlideTranslations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AppSlideTranslations_AppSlides_AppSlideId",
                table: "AppSlideTranslations",
                column: "AppSlideId",
                principalTable: "AppSlides",
                principalColumn: "Id");
        }
    }
}
