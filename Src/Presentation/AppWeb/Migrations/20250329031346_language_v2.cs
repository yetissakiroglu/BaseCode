using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AppWeb.Migrations
{
    /// <inheritdoc />
    public partial class language_v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AppLanguages",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Icon",
                table: "AppLanguages",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "AppLanguages",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "AppLanguages",
                columns: new[] { "Id", "Code", "CreatedAt", "CreatedBy", "Icon", "IsActive", "IsDefault", "IsDeleted", "IsRTL", "Name", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, "tr", new DateTime(2025, 3, 29, 6, 13, 45, 526, DateTimeKind.Local).AddTicks(6676), "", "🇹🇷", true, true, false, false, "Türkçe", null, "" },
                    { 2, "en", new DateTime(2025, 3, 29, 6, 13, 45, 528, DateTimeKind.Local).AddTicks(1232), "", "🇬🇧", true, false, false, false, "English", null, "" },
                    { 3, "ar", new DateTime(2025, 3, 29, 6, 13, 45, 528, DateTimeKind.Local).AddTicks(1242), "", "🇸🇦", true, false, false, true, "العربية", null, "" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AppLanguages",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AppLanguages",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AppLanguages",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AppLanguages",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Icon",
                table: "AppLanguages",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "AppLanguages",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);
        }
    }
}
