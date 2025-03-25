using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppWeb.Migrations
{
    /// <inheritdoc />
    public partial class AppContext_V6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppPageSection_AppPages_AppPageId",
                table: "AppPageSection");

            migrationBuilder.DropForeignKey(
                name: "FK_AppPageSection_AppSection_AppSectionId",
                table: "AppPageSection");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppSection",
                table: "AppSection");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppPageSection",
                table: "AppPageSection");

            migrationBuilder.RenameTable(
                name: "AppSection",
                newName: "AppSections");

            migrationBuilder.RenameTable(
                name: "AppPageSection",
                newName: "AppPageSections");

            migrationBuilder.RenameIndex(
                name: "IX_AppPageSection_AppSectionId",
                table: "AppPageSections",
                newName: "IX_AppPageSections_AppSectionId");

            migrationBuilder.RenameIndex(
                name: "IX_AppPageSection_AppPageId",
                table: "AppPageSections",
                newName: "IX_AppPageSections_AppPageId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppSections",
                table: "AppSections",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppPageSections",
                table: "AppPageSections",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppPageSections_AppPages_AppPageId",
                table: "AppPageSections",
                column: "AppPageId",
                principalTable: "AppPages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppPageSections_AppSections_AppSectionId",
                table: "AppPageSections",
                column: "AppSectionId",
                principalTable: "AppSections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppPageSections_AppPages_AppPageId",
                table: "AppPageSections");

            migrationBuilder.DropForeignKey(
                name: "FK_AppPageSections_AppSections_AppSectionId",
                table: "AppPageSections");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppSections",
                table: "AppSections");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppPageSections",
                table: "AppPageSections");

            migrationBuilder.RenameTable(
                name: "AppSections",
                newName: "AppSection");

            migrationBuilder.RenameTable(
                name: "AppPageSections",
                newName: "AppPageSection");

            migrationBuilder.RenameIndex(
                name: "IX_AppPageSections_AppSectionId",
                table: "AppPageSection",
                newName: "IX_AppPageSection_AppSectionId");

            migrationBuilder.RenameIndex(
                name: "IX_AppPageSections_AppPageId",
                table: "AppPageSection",
                newName: "IX_AppPageSection_AppPageId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppSection",
                table: "AppSection",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppPageSection",
                table: "AppPageSection",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppPageSection_AppPages_AppPageId",
                table: "AppPageSection",
                column: "AppPageId",
                principalTable: "AppPages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppPageSection_AppSection_AppSectionId",
                table: "AppPageSection",
                column: "AppSectionId",
                principalTable: "AppSection",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
