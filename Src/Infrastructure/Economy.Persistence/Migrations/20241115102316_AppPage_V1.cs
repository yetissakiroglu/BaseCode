using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Economy.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AppPage_V1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "AppPages");

            migrationBuilder.RenameColumn(
                name: "Url",
                table: "AppPages",
                newName: "Slug");

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "AppPages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MetaDescription",
                table: "AppPages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MetaTitle",
                table: "AppPages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShortDescription",
                table: "AppPages",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Content",
                table: "AppPages");

            migrationBuilder.DropColumn(
                name: "MetaDescription",
                table: "AppPages");

            migrationBuilder.DropColumn(
                name: "MetaTitle",
                table: "AppPages");

            migrationBuilder.DropColumn(
                name: "ShortDescription",
                table: "AppPages");

            migrationBuilder.RenameColumn(
                name: "Slug",
                table: "AppPages",
                newName: "Url");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "AppPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
