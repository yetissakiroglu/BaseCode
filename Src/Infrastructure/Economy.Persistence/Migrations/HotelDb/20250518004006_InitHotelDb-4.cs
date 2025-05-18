using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Economy.Base.Persistence.Migrations.HotelDb
{
    /// <inheritdoc />
    public partial class InitHotelDb4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "AppSettingReservationLinks",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.CreateTable(
                name: "AppSlides",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Sequence = table.Column<int>(type: "int", nullable: false),
                    ThumbnailBase64 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ThumbnailMobilBase64 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSlides", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppSlideTranslations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppSlideId = table.Column<int>(type: "int", nullable: false),
                    AppLanguageId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsExternal = table.Column<bool>(type: "bit", nullable: false),
                    ButtonText = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ButtonUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ButtonIcon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSlideTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppSlideTranslations_AppSlides_AppSlideId",
                        column: x => x.AppSlideId,
                        principalTable: "AppSlides",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AppSettingReservationLinks",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsDeleted",
                value: false);

            migrationBuilder.InsertData(
                table: "AppSlides",
                columns: new[] { "Id", "IsDeleted", "Sequence", "ThumbnailBase64", "ThumbnailMobilBase64" },
                values: new object[,]
                {
                    { 1, false, 1, "data:image/png;base64,AAA_SLIDE1", "data:image/png;base64,AAA_SLIDE1_MOBILE" },
                    { 2, false, 2, "data:image/png;base64,BBB_SLIDE2", "data:image/png;base64,BBB_SLIDE2_MOBILE" },
                    { 3, false, 3, "data:image/png;base64,CCC_SLIDE3", "data:image/png;base64,CCC_SLIDE3_MOBILE" }
                });

            migrationBuilder.InsertData(
                table: "AppSlideTranslations",
                columns: new[] { "Id", "AppLanguageId", "AppSlideId", "ButtonIcon", "ButtonText", "ButtonUrl", "Content", "IsDeleted", "IsExternal", "Title" },
                values: new object[,]
                {
                    { 1, 1, 1, "info", "Explore", "/about", "Welcome to our website!", false, false, "Welcome" },
                    { 2, 2, 1, "info", "Keşfet", "/hakkimizda", "Web sitemize hoş geldiniz!", false, false, "Hoş Geldiniz" },
                    { 3, 1, 2, "service", "View Services", "/services", "See what we offer for you.", false, false, "Our Services" },
                    { 4, 2, 2, "service", "Hizmetleri Gör", "/hizmetler", "Sunduğumuz hizmetleri keşfedin.", false, false, "Hizmetlerimiz" },
                    { 5, 1, 3, "mail", "Get in Touch", "https://contact.example.com", "We are here to help.", false, true, "Contact Us" },
                    { 6, 2, 3, "mail", "İletişime Geç", "https://iletisim.example.com", "Size yardımcı olmak için buradayız.", false, true, "Bize Ulaşın" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppSlideTranslations_AppSlideId",
                table: "AppSlideTranslations",
                column: "AppSlideId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppSlideTranslations");

            migrationBuilder.DropTable(
                name: "AppSlides");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "AppSettingReservationLinks",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.UpdateData(
                table: "AppSettingReservationLinks",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsDeleted",
                value: true);
        }
    }
}
