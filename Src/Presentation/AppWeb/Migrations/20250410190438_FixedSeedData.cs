using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppWeb.Migrations
{
    /// <inheritdoc />
    public partial class FixedSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AppRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { 1, "a1111111-b222-c333-d444-e55555555555", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AppUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "DateOfBirth", "Email", "EmailConfirmed", "FirstName", "IsDefaultAdmin", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { 1, 0, "33333333-dddd-eeee-ffff-444444444444", new DateTime(1990, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@example.com", true, "Sistem", true, "Yöneticisi", false, null, "ADMIN@EXAMPLE.COM", "ADMIN", "AQAAAAEAACcQAAAAEGCzY20L2G+TswPL8nVZ7gCm+3OaKjk9iN9abVdTOf5wjMPVnljfMRZsWYixI4LSQg==", null, false, "11111111-aaaa-bbbb-cccc-222222222222", false, "admin" });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { 1, 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "AppRoles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
