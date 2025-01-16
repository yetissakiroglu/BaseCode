using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Economy.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class first_v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppPageSectionTranslations");

            migrationBuilder.DropTable(
                name: "AppPageTranslations");

            migrationBuilder.DropTable(
                name: "AppSectionTranslations");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AppSections",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "AppPageSections",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsVisible",
                table: "AppPageSections",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AppPageSections",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "AppPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "AppPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "AppPages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "AppSections");

            migrationBuilder.DropColumn(
                name: "Content",
                table: "AppPageSections");

            migrationBuilder.DropColumn(
                name: "IsVisible",
                table: "AppPageSections");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "AppPageSections");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "AppPages");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "AppPages");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "AppPages");

            migrationBuilder.CreateTable(
                name: "AppPageSectionTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppPageSectionId = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    LanguageCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppPageSectionTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppPageSectionTranslations_AppPageSections_AppPageSectionId",
                        column: x => x.AppPageSectionId,
                        principalTable: "AppPageSections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppPageTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppPageId = table.Column<int>(type: "int", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LanguageCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppPageTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppPageTranslations_AppPages_AppPageId",
                        column: x => x.AppPageId,
                        principalTable: "AppPages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppSectionTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppSectionId = table.Column<int>(type: "int", nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LanguageCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSectionTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppSectionTranslations_AppSections_AppSectionId",
                        column: x => x.AppSectionId,
                        principalTable: "AppSections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppPageSectionTranslations_AppPageSectionId",
                table: "AppPageSectionTranslations",
                column: "AppPageSectionId");

            migrationBuilder.CreateIndex(
                name: "IX_AppPageTranslations_AppPageId",
                table: "AppPageTranslations",
                column: "AppPageId");

            migrationBuilder.CreateIndex(
                name: "IX_AppSectionTranslations_AppSectionId",
                table: "AppSectionTranslations",
                column: "AppSectionId");
        }
    }
}
