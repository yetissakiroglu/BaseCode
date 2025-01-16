using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Economy.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CategoryPublicationStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PublicationStatus",
                table: "AppCategories",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublicationStatus",
                table: "AppCategories");
        }
    }
}
