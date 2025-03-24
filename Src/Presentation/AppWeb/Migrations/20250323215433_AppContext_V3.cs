using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppWeb.Migrations
{
    /// <inheritdoc />
    public partial class AppContext_V3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppCategory_AppCategory_ParentCategoryId",
                table: "AppCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_AppContents_AppCategory_AppCategoryId",
                table: "AppContents");

            migrationBuilder.DropForeignKey(
                name: "FK_AppContents_AspNetUsers_AppUserId",
                table: "AppContents");

            migrationBuilder.DropTable(
                name: "AppAmenity");

            migrationBuilder.DropTable(
                name: "AppComment");

            migrationBuilder.DropTable(
                name: "AppContent_Document");

            migrationBuilder.DropTable(
                name: "AppContent_ImportantNote");

            migrationBuilder.DropTable(
                name: "AppContentTag");

            migrationBuilder.DropTable(
                name: "AppImage");

            migrationBuilder.DropTable(
                name: "AppAmenityGroup");

            migrationBuilder.DropTable(
                name: "AppTag");

            migrationBuilder.DropTable(
                name: "AppImageGroup");

            migrationBuilder.DropIndex(
                name: "IX_AppContents_AppUserId",
                table: "AppContents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppCategory",
                table: "AppCategory");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "AppContents");

            migrationBuilder.DropColumn(
                name: "ApprovalStatus",
                table: "AppContents");

            migrationBuilder.DropColumn(
                name: "Author",
                table: "AppContents");

            migrationBuilder.DropColumn(
                name: "FeaturedImage",
                table: "AppContents");

            migrationBuilder.DropColumn(
                name: "IsBreakingNews",
                table: "AppContents");

            migrationBuilder.DropColumn(
                name: "IsFeatured",
                table: "AppContents");

            migrationBuilder.DropColumn(
                name: "IsHeadline",
                table: "AppContents");

            migrationBuilder.DropColumn(
                name: "MetaKeywords",
                table: "AppContents");

            migrationBuilder.DropColumn(
                name: "ViewCount",
                table: "AppContents");

            migrationBuilder.RenameTable(
                name: "AppCategory",
                newName: "AppCategories");

            migrationBuilder.RenameColumn(
                name: "Slug",
                table: "AppContents",
                newName: "Url");

            migrationBuilder.RenameColumn(
                name: "Slug",
                table: "AppCategories",
                newName: "Url");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "AppCategories",
                newName: "ShortDescription");

            migrationBuilder.RenameIndex(
                name: "IX_AppCategory_ParentCategoryId",
                table: "AppCategories",
                newName: "IX_AppCategories_ParentCategoryId");

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "AppCategories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppCategories",
                table: "AppCategories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppCategories_AppCategories_ParentCategoryId",
                table: "AppCategories",
                column: "ParentCategoryId",
                principalTable: "AppCategories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppContents_AppCategories_AppCategoryId",
                table: "AppContents",
                column: "AppCategoryId",
                principalTable: "AppCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppCategories_AppCategories_ParentCategoryId",
                table: "AppCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_AppContents_AppCategories_AppCategoryId",
                table: "AppContents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppCategories",
                table: "AppCategories");

            migrationBuilder.DropColumn(
                name: "Content",
                table: "AppCategories");

            migrationBuilder.RenameTable(
                name: "AppCategories",
                newName: "AppCategory");

            migrationBuilder.RenameColumn(
                name: "Url",
                table: "AppContents",
                newName: "Slug");

            migrationBuilder.RenameColumn(
                name: "Url",
                table: "AppCategory",
                newName: "Slug");

            migrationBuilder.RenameColumn(
                name: "ShortDescription",
                table: "AppCategory",
                newName: "Description");

            migrationBuilder.RenameIndex(
                name: "IX_AppCategories_ParentCategoryId",
                table: "AppCategory",
                newName: "IX_AppCategory_ParentCategoryId");

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "AppContents",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ApprovalStatus",
                table: "AppContents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "AppContents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FeaturedImage",
                table: "AppContents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsBreakingNews",
                table: "AppContents",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsFeatured",
                table: "AppContents",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsHeadline",
                table: "AppContents",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "MetaKeywords",
                table: "AppContents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ViewCount",
                table: "AppContents",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppCategory",
                table: "AppCategory",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "AppAmenityGroup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppContentId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppAmenityGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppAmenityGroup_AppContents_AppContentId",
                        column: x => x.AppContentId,
                        principalTable: "AppContents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppComment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppContentId = table.Column<int>(type: "int", nullable: false),
                    AppUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ApprovalStatus = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppComment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppComment_AppContents_AppContentId",
                        column: x => x.AppContentId,
                        principalTable: "AppContents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppComment_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AppContent_Document",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppContentId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DocumentId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppContent_Document", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppContent_Document_AppContents_AppContentId",
                        column: x => x.AppContentId,
                        principalTable: "AppContents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppContent_ImportantNote",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppContentId = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Sequence = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppContent_ImportantNote", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppContent_ImportantNote_AppContents_AppContentId",
                        column: x => x.AppContentId,
                        principalTable: "AppContents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppImageGroup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppContentId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppImageGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppImageGroup_AppContents_AppContentId",
                        column: x => x.AppContentId,
                        principalTable: "AppContents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppTag",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppTag", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppAmenity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppAmenityGroupId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppAmenity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppAmenity_AppAmenityGroup_AppAmenityGroupId",
                        column: x => x.AppAmenityGroupId,
                        principalTable: "AppAmenityGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppImage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppImageGroupId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsCover = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppImage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppImage_AppImageGroup_AppImageGroupId",
                        column: x => x.AppImageGroupId,
                        principalTable: "AppImageGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppContentTag",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppContentId = table.Column<int>(type: "int", nullable: false),
                    AppTagId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppContentTag", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppContentTag_AppContents_AppContentId",
                        column: x => x.AppContentId,
                        principalTable: "AppContents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppContentTag_AppTag_AppTagId",
                        column: x => x.AppTagId,
                        principalTable: "AppTag",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppContents_AppUserId",
                table: "AppContents",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAmenity_AppAmenityGroupId",
                table: "AppAmenity",
                column: "AppAmenityGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_AppAmenityGroup_AppContentId",
                table: "AppAmenityGroup",
                column: "AppContentId");

            migrationBuilder.CreateIndex(
                name: "IX_AppComment_AppContentId",
                table: "AppComment",
                column: "AppContentId");

            migrationBuilder.CreateIndex(
                name: "IX_AppComment_AppUserId",
                table: "AppComment",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AppContent_Document_AppContentId",
                table: "AppContent_Document",
                column: "AppContentId");

            migrationBuilder.CreateIndex(
                name: "IX_AppContent_ImportantNote_AppContentId",
                table: "AppContent_ImportantNote",
                column: "AppContentId");

            migrationBuilder.CreateIndex(
                name: "IX_AppContentTag_AppContentId",
                table: "AppContentTag",
                column: "AppContentId");

            migrationBuilder.CreateIndex(
                name: "IX_AppContentTag_AppTagId",
                table: "AppContentTag",
                column: "AppTagId");

            migrationBuilder.CreateIndex(
                name: "IX_AppImage_AppImageGroupId",
                table: "AppImage",
                column: "AppImageGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_AppImageGroup_AppContentId",
                table: "AppImageGroup",
                column: "AppContentId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppCategory_AppCategory_ParentCategoryId",
                table: "AppCategory",
                column: "ParentCategoryId",
                principalTable: "AppCategory",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppContents_AppCategory_AppCategoryId",
                table: "AppContents",
                column: "AppCategoryId",
                principalTable: "AppCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppContents_AspNetUsers_AppUserId",
                table: "AppContents",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
