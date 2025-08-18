using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Economy.Panel.UI.Migrations
{
    /// <inheritdoc />
    public partial class MigrationDefaultDb_setting_v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppSecuritySettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PasswordRequiredLength = table.Column<int>(type: "int", nullable: false, defaultValue: 6),
                    PasswordRequireDigit = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    PasswordRequireLowercase = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    PasswordRequireUppercase = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    PasswordRequireNonAlphanumeric = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    LockoutTimeSpanMinutes = table.Column<int>(type: "int", nullable: false, defaultValue: 30),
                    LockoutMaxFailedAccessAttempts = table.Column<int>(type: "int", nullable: false, defaultValue: 5),
                    LockoutAllowedForNewUsers = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    SignInRequireConfirmedEmail = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    SignInRequireConfirmedPhoneNumber = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    TwoFactorRequired = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSecuritySettings", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppSecuritySettings");
        }
    }
}
