using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Economy.Panel.UI.Migrations
{
    /// <inheritdoc />
    public partial class MigrationDefaultDb_setting_v1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppGeneralSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SiteName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Domain = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Theme = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "light"),
                    LogoUrl = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    MetaTitleSuffix = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true),
                    DefaultMetaDescription = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppGeneralSettings", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppGeneralSettings");
        }
    }
}
