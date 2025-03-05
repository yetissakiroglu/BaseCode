using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppWeb.Migrations
{
    /// <inheritdoc />
    public partial class AppSetting_Language_Create_v4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactEmail",
                table: "AppSettings");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "AppSettings",
                newName: "IsMaintenanceMode");

            migrationBuilder.RenameColumn(
                name: "ContactPhone",
                table: "AppSettings",
                newName: "MaintenanceMessage");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MaintenanceMessage",
                table: "AppSettings",
                newName: "ContactPhone");

            migrationBuilder.RenameColumn(
                name: "IsMaintenanceMode",
                table: "AppSettings",
                newName: "IsActive");

            migrationBuilder.AddColumn<string>(
                name: "ContactEmail",
                table: "AppSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
