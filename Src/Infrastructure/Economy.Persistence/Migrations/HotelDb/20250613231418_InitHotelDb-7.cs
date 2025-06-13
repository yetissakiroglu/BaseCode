using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Economy.Base.Persistence.Migrations.HotelDb
{
    /// <inheritdoc />
    public partial class InitHotelDb7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppContents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WebThumbnailUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    MobilThumbnailUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ContentType = table.Column<int>(type: "int", nullable: false),
                    AppCategoryId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppContents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppContents_AppCategories_AppCategoryId",
                        column: x => x.AppCategoryId,
                        principalTable: "AppCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "AppContentTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppContentId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    ShortDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsExternal = table.Column<bool>(type: "bit", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    MetaTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MetaDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AppLanguageId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppContentTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppContentTranslations_AppContents_AppContentId",
                        column: x => x.AppContentId,
                        principalTable: "AppContents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AppContents",
                columns: new[] { "Id", "AppCategoryId", "ContentType", "IsDeleted", "MobilThumbnailUrl", "WebThumbnailUrl" },
                values: new object[,]
                {
                    { 1, null, 2, false, "/images/content1-mobile.jpg", "/images/content1-web.jpg" },
                    { 2, 1, 3, false, "/images/content2-mobile.jpg", "/images/content2-web.jpg" }
                });

            migrationBuilder.InsertData(
                table: "AppContentTranslations",
                columns: new[] { "Id", "AppContentId", "AppLanguageId", "Content", "IsDeleted", "IsExternal", "MetaDescription", "MetaTitle", "ShortDescription", "Title", "Url" },
                values: new object[,]
                {
                    { 1, 1, 1, "Detaylı içerik 1", false, false, "Oda 1 açıklaması", "Oda 1 SEO", "Kısa açıklama 1", "Oda 1", "/oda-1" },
                    { 2, 2, 1, "Şirket hakkında detaylı bilgi", false, false, "Hakkımızda açıklaması", "Hakkımızda SEO", "Kurumsal kısa açıklama", "Hakkımızda", "/hakkimizda" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppContents_AppCategoryId",
                table: "AppContents",
                column: "AppCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_AppContentTranslations_AppContentId",
                table: "AppContentTranslations",
                column: "AppContentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppContentTranslations");

            migrationBuilder.DropTable(
                name: "AppContents");
        }
    }
}
