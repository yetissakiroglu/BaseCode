using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Economy.Panel.UI.Migrations
{
    /// <inheritdoc />
    public partial class MigrationDefaultDbFirst_App_TalentId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "Apps",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Apps",
                keyColumn: "Id",
                keyValue: 1,
                column: "TenantId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Apps",
                keyColumn: "Id",
                keyValue: 2,
                column: "TenantId",
                value: null);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { 2, "f6666666-g777-h888-i999-j00000000000", "Otel Editör", "OTEL EDİTÖR" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Email", "FirstName", "JobTitle", "LastName", "NormalizedEmail", "NormalizedUserName", "TenantId", "UserName" },
                values: new object[] { "yetissakiroglu@gmail.com", "Yetiş", "Süper Admin", "Şakiroğlu", "YETISSAKIROGLU@GMAIL.COM", "Admin", 0, "Admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Apps");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Email", "FirstName", "JobTitle", "LastName", "NormalizedEmail", "NormalizedUserName", "TenantId", "UserName" },
                values: new object[] { "Hotel1@example.com", "Hotel1", null, "Yöneticisi", "HOTEL1@EXAMPLE.COM", "Hotel1", 1, "Hotel1" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "IsDefaultAdmin", "IsDeleted", "JobTitle", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PhotoUrl", "SecurityStamp", "TenantId", "TwoFactorEnabled", "UserName" },
                values: new object[] { 2, 0, "33333333-dddd-eeee-ffff-444444444444", "Hotel2@example.com", true, "Hotel2", true, false, null, "Yöneticisi", false, null, "HOTEL2@EXAMPLE.COM", "Hotel2", "AQAAAAIAAYagAAAAEGhEU2J20Dt9rbBXKRMbF5MaTTD8UzKKRrYn+gZZfQsOImpHd+x/0sY1AA++BQV4Xw==", null, false, null, "11111111-aaaa-bbbb-cccc-222222222222", 2, false, "Hotel2" });
        }
    }
}
