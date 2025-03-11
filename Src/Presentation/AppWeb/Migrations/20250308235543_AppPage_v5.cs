using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppWeb.Migrations
{
    /// <inheritdoc />
    public partial class AppPage_v5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShortDescription",
                table: "AppPageTranslations");

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "AppPageTranslations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Url",
                table: "AppPageTranslations");

            migrationBuilder.AddColumn<string>(
                name: "ShortDescription",
                table: "AppPageTranslations",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
