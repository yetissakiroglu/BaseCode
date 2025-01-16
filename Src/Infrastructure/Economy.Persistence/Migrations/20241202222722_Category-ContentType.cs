using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Economy.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CategoryContentType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CanonicalUrl",
                table: "AppCategories");

            migrationBuilder.AddColumn<int>(
                name: "ContentType",
                table: "AppCategories",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "AppCategories");

            migrationBuilder.AddColumn<string>(
                name: "CanonicalUrl",
                table: "AppCategories",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);
        }
    }
}
