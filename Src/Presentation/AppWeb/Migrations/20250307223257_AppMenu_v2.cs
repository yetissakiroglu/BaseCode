using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppWeb.Migrations
{
    /// <inheritdoc />
    public partial class AppMenu_v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Slug",
                table: "AppMenus");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "AppMenus");

            migrationBuilder.CreateTable(
                name: "AppMenuTranslation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppLanguageId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AppMenuId = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppMenuTranslation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppMenuTranslation_AppLanguages_AppLanguageId",
                        column: x => x.AppLanguageId,
                        principalTable: "AppLanguages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppMenuTranslation_AppMenus_AppMenuId",
                        column: x => x.AppMenuId,
                        principalTable: "AppMenus",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppMenuTranslation_AppLanguageId",
                table: "AppMenuTranslation",
                column: "AppLanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_AppMenuTranslation_AppMenuId",
                table: "AppMenuTranslation",
                column: "AppMenuId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppMenuTranslation");

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "AppMenus",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "AppMenus",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }
    }
}
