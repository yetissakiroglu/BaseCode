using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppWeb.Migrations
{
    /// <inheritdoc />
    public partial class logo_v1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppLogoSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WebLogoPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    MobileLogoPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppLogoSettings", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AppLogoSettings",
                columns: new[] { "Id", "IsDeleted", "MobileLogoPath", "WebLogoPath" },
                values: new object[] { 1, false, "img/logo_m.png", "img/logo.png" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppLogoSettings");
        }
    }
}
