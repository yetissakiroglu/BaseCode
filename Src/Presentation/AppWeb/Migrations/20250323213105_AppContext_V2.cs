using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppWeb.Migrations
{
    /// <inheritdoc />
    public partial class AppContext_V2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppAmenities_AppAmenityGroups_AppAmenityGroupId",
                table: "AppAmenities");

            migrationBuilder.DropForeignKey(
                name: "FK_AppAmenityGroups_AppContents_AppContentId",
                table: "AppAmenityGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_AppCategories_AppCategories_ParentCategoryId",
                table: "AppCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_AppComments_AppContents_AppContentId",
                table: "AppComments");

            migrationBuilder.DropForeignKey(
                name: "FK_AppComments_AspNetUsers_AppUserId",
                table: "AppComments");

            migrationBuilder.DropForeignKey(
                name: "FK_AppContent_Documents_AppContents_AppContentId",
                table: "AppContent_Documents");

            migrationBuilder.DropForeignKey(
                name: "FK_AppContent_ImportantNotes_AppContents_AppContentId",
                table: "AppContent_ImportantNotes");

            migrationBuilder.DropForeignKey(
                name: "FK_AppContents_AppCategories_AppCategoryId",
                table: "AppContents");

            migrationBuilder.DropForeignKey(
                name: "FK_AppContentTags_AppContents_AppContentId",
                table: "AppContentTags");

            migrationBuilder.DropForeignKey(
                name: "FK_AppContentTags_AppTags_AppTagId",
                table: "AppContentTags");

            migrationBuilder.DropForeignKey(
                name: "FK_AppImageGroups_AppContents_AppContentId",
                table: "AppImageGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_AppImages_AppImageGroups_AppImageGroupId",
                table: "AppImages");

            migrationBuilder.DropTable(
                name: "AppFileDocuments");

            migrationBuilder.DropTable(
                name: "AppFileImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppTags",
                table: "AppTags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppImages",
                table: "AppImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppImageGroups",
                table: "AppImageGroups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppContentTags",
                table: "AppContentTags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppContent_ImportantNotes",
                table: "AppContent_ImportantNotes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppContent_Documents",
                table: "AppContent_Documents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppComments",
                table: "AppComments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppCategories",
                table: "AppCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppAmenityGroups",
                table: "AppAmenityGroups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppAmenities",
                table: "AppAmenities");

            migrationBuilder.RenameTable(
                name: "AppTags",
                newName: "AppTag");

            migrationBuilder.RenameTable(
                name: "AppImages",
                newName: "AppImage");

            migrationBuilder.RenameTable(
                name: "AppImageGroups",
                newName: "AppImageGroup");

            migrationBuilder.RenameTable(
                name: "AppContentTags",
                newName: "AppContentTag");

            migrationBuilder.RenameTable(
                name: "AppContent_ImportantNotes",
                newName: "AppContent_ImportantNote");

            migrationBuilder.RenameTable(
                name: "AppContent_Documents",
                newName: "AppContent_Document");

            migrationBuilder.RenameTable(
                name: "AppComments",
                newName: "AppComment");

            migrationBuilder.RenameTable(
                name: "AppCategories",
                newName: "AppCategory");

            migrationBuilder.RenameTable(
                name: "AppAmenityGroups",
                newName: "AppAmenityGroup");

            migrationBuilder.RenameTable(
                name: "AppAmenities",
                newName: "AppAmenity");

            migrationBuilder.RenameIndex(
                name: "IX_AppImages_AppImageGroupId",
                table: "AppImage",
                newName: "IX_AppImage_AppImageGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_AppImageGroups_AppContentId",
                table: "AppImageGroup",
                newName: "IX_AppImageGroup_AppContentId");

            migrationBuilder.RenameIndex(
                name: "IX_AppContentTags_AppTagId",
                table: "AppContentTag",
                newName: "IX_AppContentTag_AppTagId");

            migrationBuilder.RenameIndex(
                name: "IX_AppContentTags_AppContentId",
                table: "AppContentTag",
                newName: "IX_AppContentTag_AppContentId");

            migrationBuilder.RenameIndex(
                name: "IX_AppContent_ImportantNotes_AppContentId",
                table: "AppContent_ImportantNote",
                newName: "IX_AppContent_ImportantNote_AppContentId");

            migrationBuilder.RenameIndex(
                name: "IX_AppContent_Documents_AppContentId",
                table: "AppContent_Document",
                newName: "IX_AppContent_Document_AppContentId");

            migrationBuilder.RenameIndex(
                name: "IX_AppComments_AppUserId",
                table: "AppComment",
                newName: "IX_AppComment_AppUserId");

            migrationBuilder.RenameIndex(
                name: "IX_AppComments_AppContentId",
                table: "AppComment",
                newName: "IX_AppComment_AppContentId");

            migrationBuilder.RenameIndex(
                name: "IX_AppCategories_ParentCategoryId",
                table: "AppCategory",
                newName: "IX_AppCategory_ParentCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_AppAmenityGroups_AppContentId",
                table: "AppAmenityGroup",
                newName: "IX_AppAmenityGroup_AppContentId");

            migrationBuilder.RenameIndex(
                name: "IX_AppAmenities_AppAmenityGroupId",
                table: "AppAmenity",
                newName: "IX_AppAmenity_AppAmenityGroupId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppTag",
                table: "AppTag",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppImage",
                table: "AppImage",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppImageGroup",
                table: "AppImageGroup",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppContentTag",
                table: "AppContentTag",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppContent_ImportantNote",
                table: "AppContent_ImportantNote",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppContent_Document",
                table: "AppContent_Document",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppComment",
                table: "AppComment",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppCategory",
                table: "AppCategory",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppAmenityGroup",
                table: "AppAmenityGroup",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppAmenity",
                table: "AppAmenity",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppAmenity_AppAmenityGroup_AppAmenityGroupId",
                table: "AppAmenity",
                column: "AppAmenityGroupId",
                principalTable: "AppAmenityGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppAmenityGroup_AppContents_AppContentId",
                table: "AppAmenityGroup",
                column: "AppContentId",
                principalTable: "AppContents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppCategory_AppCategory_ParentCategoryId",
                table: "AppCategory",
                column: "ParentCategoryId",
                principalTable: "AppCategory",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppComment_AppContents_AppContentId",
                table: "AppComment",
                column: "AppContentId",
                principalTable: "AppContents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppComment_AspNetUsers_AppUserId",
                table: "AppComment",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppContent_Document_AppContents_AppContentId",
                table: "AppContent_Document",
                column: "AppContentId",
                principalTable: "AppContents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppContent_ImportantNote_AppContents_AppContentId",
                table: "AppContent_ImportantNote",
                column: "AppContentId",
                principalTable: "AppContents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppContents_AppCategory_AppCategoryId",
                table: "AppContents",
                column: "AppCategoryId",
                principalTable: "AppCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppContentTag_AppContents_AppContentId",
                table: "AppContentTag",
                column: "AppContentId",
                principalTable: "AppContents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppContentTag_AppTag_AppTagId",
                table: "AppContentTag",
                column: "AppTagId",
                principalTable: "AppTag",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppImage_AppImageGroup_AppImageGroupId",
                table: "AppImage",
                column: "AppImageGroupId",
                principalTable: "AppImageGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppImageGroup_AppContents_AppContentId",
                table: "AppImageGroup",
                column: "AppContentId",
                principalTable: "AppContents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppAmenity_AppAmenityGroup_AppAmenityGroupId",
                table: "AppAmenity");

            migrationBuilder.DropForeignKey(
                name: "FK_AppAmenityGroup_AppContents_AppContentId",
                table: "AppAmenityGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_AppCategory_AppCategory_ParentCategoryId",
                table: "AppCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_AppComment_AppContents_AppContentId",
                table: "AppComment");

            migrationBuilder.DropForeignKey(
                name: "FK_AppComment_AspNetUsers_AppUserId",
                table: "AppComment");

            migrationBuilder.DropForeignKey(
                name: "FK_AppContent_Document_AppContents_AppContentId",
                table: "AppContent_Document");

            migrationBuilder.DropForeignKey(
                name: "FK_AppContent_ImportantNote_AppContents_AppContentId",
                table: "AppContent_ImportantNote");

            migrationBuilder.DropForeignKey(
                name: "FK_AppContents_AppCategory_AppCategoryId",
                table: "AppContents");

            migrationBuilder.DropForeignKey(
                name: "FK_AppContentTag_AppContents_AppContentId",
                table: "AppContentTag");

            migrationBuilder.DropForeignKey(
                name: "FK_AppContentTag_AppTag_AppTagId",
                table: "AppContentTag");

            migrationBuilder.DropForeignKey(
                name: "FK_AppImage_AppImageGroup_AppImageGroupId",
                table: "AppImage");

            migrationBuilder.DropForeignKey(
                name: "FK_AppImageGroup_AppContents_AppContentId",
                table: "AppImageGroup");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppTag",
                table: "AppTag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppImageGroup",
                table: "AppImageGroup");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppImage",
                table: "AppImage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppContentTag",
                table: "AppContentTag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppContent_ImportantNote",
                table: "AppContent_ImportantNote");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppContent_Document",
                table: "AppContent_Document");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppComment",
                table: "AppComment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppCategory",
                table: "AppCategory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppAmenityGroup",
                table: "AppAmenityGroup");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppAmenity",
                table: "AppAmenity");

            migrationBuilder.RenameTable(
                name: "AppTag",
                newName: "AppTags");

            migrationBuilder.RenameTable(
                name: "AppImageGroup",
                newName: "AppImageGroups");

            migrationBuilder.RenameTable(
                name: "AppImage",
                newName: "AppImages");

            migrationBuilder.RenameTable(
                name: "AppContentTag",
                newName: "AppContentTags");

            migrationBuilder.RenameTable(
                name: "AppContent_ImportantNote",
                newName: "AppContent_ImportantNotes");

            migrationBuilder.RenameTable(
                name: "AppContent_Document",
                newName: "AppContent_Documents");

            migrationBuilder.RenameTable(
                name: "AppComment",
                newName: "AppComments");

            migrationBuilder.RenameTable(
                name: "AppCategory",
                newName: "AppCategories");

            migrationBuilder.RenameTable(
                name: "AppAmenityGroup",
                newName: "AppAmenityGroups");

            migrationBuilder.RenameTable(
                name: "AppAmenity",
                newName: "AppAmenities");

            migrationBuilder.RenameIndex(
                name: "IX_AppImageGroup_AppContentId",
                table: "AppImageGroups",
                newName: "IX_AppImageGroups_AppContentId");

            migrationBuilder.RenameIndex(
                name: "IX_AppImage_AppImageGroupId",
                table: "AppImages",
                newName: "IX_AppImages_AppImageGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_AppContentTag_AppTagId",
                table: "AppContentTags",
                newName: "IX_AppContentTags_AppTagId");

            migrationBuilder.RenameIndex(
                name: "IX_AppContentTag_AppContentId",
                table: "AppContentTags",
                newName: "IX_AppContentTags_AppContentId");

            migrationBuilder.RenameIndex(
                name: "IX_AppContent_ImportantNote_AppContentId",
                table: "AppContent_ImportantNotes",
                newName: "IX_AppContent_ImportantNotes_AppContentId");

            migrationBuilder.RenameIndex(
                name: "IX_AppContent_Document_AppContentId",
                table: "AppContent_Documents",
                newName: "IX_AppContent_Documents_AppContentId");

            migrationBuilder.RenameIndex(
                name: "IX_AppComment_AppUserId",
                table: "AppComments",
                newName: "IX_AppComments_AppUserId");

            migrationBuilder.RenameIndex(
                name: "IX_AppComment_AppContentId",
                table: "AppComments",
                newName: "IX_AppComments_AppContentId");

            migrationBuilder.RenameIndex(
                name: "IX_AppCategory_ParentCategoryId",
                table: "AppCategories",
                newName: "IX_AppCategories_ParentCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_AppAmenityGroup_AppContentId",
                table: "AppAmenityGroups",
                newName: "IX_AppAmenityGroups_AppContentId");

            migrationBuilder.RenameIndex(
                name: "IX_AppAmenity_AppAmenityGroupId",
                table: "AppAmenities",
                newName: "IX_AppAmenities_AppAmenityGroupId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppTags",
                table: "AppTags",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppImageGroups",
                table: "AppImageGroups",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppImages",
                table: "AppImages",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppContentTags",
                table: "AppContentTags",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppContent_ImportantNotes",
                table: "AppContent_ImportantNotes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppContent_Documents",
                table: "AppContent_Documents",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppComments",
                table: "AppComments",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppCategories",
                table: "AppCategories",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppAmenityGroups",
                table: "AppAmenityGroups",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppAmenities",
                table: "AppAmenities",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "AppFileDocuments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DocumentData = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    OriginalFileName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppFileDocuments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppFileImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImageData = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    OriginalFileName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppFileImages", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_AppAmenities_AppAmenityGroups_AppAmenityGroupId",
                table: "AppAmenities",
                column: "AppAmenityGroupId",
                principalTable: "AppAmenityGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppAmenityGroups_AppContents_AppContentId",
                table: "AppAmenityGroups",
                column: "AppContentId",
                principalTable: "AppContents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppCategories_AppCategories_ParentCategoryId",
                table: "AppCategories",
                column: "ParentCategoryId",
                principalTable: "AppCategories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppComments_AppContents_AppContentId",
                table: "AppComments",
                column: "AppContentId",
                principalTable: "AppContents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppComments_AspNetUsers_AppUserId",
                table: "AppComments",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppContent_Documents_AppContents_AppContentId",
                table: "AppContent_Documents",
                column: "AppContentId",
                principalTable: "AppContents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppContent_ImportantNotes_AppContents_AppContentId",
                table: "AppContent_ImportantNotes",
                column: "AppContentId",
                principalTable: "AppContents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppContents_AppCategories_AppCategoryId",
                table: "AppContents",
                column: "AppCategoryId",
                principalTable: "AppCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppContentTags_AppContents_AppContentId",
                table: "AppContentTags",
                column: "AppContentId",
                principalTable: "AppContents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppContentTags_AppTags_AppTagId",
                table: "AppContentTags",
                column: "AppTagId",
                principalTable: "AppTags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppImageGroups_AppContents_AppContentId",
                table: "AppImageGroups",
                column: "AppContentId",
                principalTable: "AppContents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppImages_AppImageGroups_AppImageGroupId",
                table: "AppImages",
                column: "AppImageGroupId",
                principalTable: "AppImageGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
