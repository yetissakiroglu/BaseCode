using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AppWeb.Migrations
{
    /// <inheritdoc />
    public partial class menu_v3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AppMenus",
                columns: new[] { "Id", "IsDeleted", "IsExternal", "ParentMenuId" },
                values: new object[,]
                {
                    { 1, false, false, null },
                    { 2, false, false, null },
                    { 3, false, false, null },
                    { 4, false, false, null },
                    { 5, false, false, null },
                    { 6, false, false, null }
                });

            migrationBuilder.InsertData(
                table: "AppMenuTranslations",
                columns: new[] { "Id", "AppLanguageId", "AppMenuId", "IsDeleted", "Title", "Url" },
                values: new object[,]
                {
                    { 1, 1, 1, false, "Ana Menü", "ana-menu" },
                    { 2, 2, 1, false, "Main Menu", "main-menu" },
                    { 3, 1, 2, false, "Odalar & Süitler", "odalar-suitler" },
                    { 4, 2, 2, false, "Rooms & Suites", "rooms-suites" },
                    { 5, 1, 3, false, "Restoran & Bar", "restoran-bar" },
                    { 6, 2, 3, false, "Restaurant & Bar", "restaurant-bar" },
                    { 7, 1, 4, false, "Spa & Wellness", "spa-wellness" },
                    { 8, 2, 4, false, "Spa & Wellness", "spa-wellness" },
                    { 9, 1, 5, false, "Hakkımızda", "hakkimizda" },
                    { 10, 2, 5, false, "About Us", "about-us" },
                    { 11, 1, 6, false, "İletişim", "iletisim" },
                    { 12, 2, 6, false, "Contact", "contact" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AppMenuTranslations",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AppMenuTranslations",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AppMenuTranslations",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AppMenuTranslations",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "AppMenuTranslations",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "AppMenuTranslations",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "AppMenuTranslations",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "AppMenuTranslations",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "AppMenuTranslations",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "AppMenuTranslations",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "AppMenuTranslations",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "AppMenuTranslations",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "AppMenus",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AppMenus",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AppMenus",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AppMenus",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "AppMenus",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "AppMenus",
                keyColumn: "Id",
                keyValue: 6);
        }
    }
}
