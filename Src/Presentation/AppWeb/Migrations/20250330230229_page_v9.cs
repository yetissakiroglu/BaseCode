using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AppWeb.Migrations
{
    /// <inheritdoc />
    public partial class page_v9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppSectionImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppSectionId = table.Column<int>(type: "int", nullable: false),
                    Sequence = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Thumbnail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSectionImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppSectionImages_AppSections_AppSectionId",
                        column: x => x.AppSectionId,
                        principalTable: "AppSections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AppSectionImages",
                columns: new[] { "Id", "AppSectionId", "Content", "IsDeleted", "Name", "Sequence", "Thumbnail" },
                values: new object[,]
                {
                    { 1, 2, "", false, "Kurumsal 1", 1, "essiz-misafirperverligi-595bb.jpg" },
                    { 2, 2, "", false, "Kurumsal 2", 2, "essiz-osmanli-stili-ve-misafirperverligi-7315a.jpg" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppSectionImages_AppSectionId",
                table: "AppSectionImages",
                column: "AppSectionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppSectionImages");
        }
    }
}
