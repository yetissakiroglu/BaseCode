using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Economy.Base.Persistence.Migrations.HotelDb
{
    /// <inheritdoc />
    public partial class InitHotelDb3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppSettingReservationLinks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSettingReservationLinks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppSettingReservationNumbers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountryCode = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Number = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSettingReservationNumbers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppSettingWhatsappLines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountryCode = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Number = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSettingWhatsappLines", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AppSettingReservationLinks",
                columns: new[] { "Id", "Url" },
                values: new object[] { 1, "https://example.com/reservation" });

            migrationBuilder.InsertData(
                table: "AppSettingReservationNumbers",
                columns: new[] { "Id", "CountryCode", "IsPrimary", "Number" },
                values: new object[,]
                {
                    { 1, "+90", true, "5551112233" },
                    { 2, "+1", false, "2025550123" }
                });

            migrationBuilder.InsertData(
                table: "AppSettingWhatsappLines",
                columns: new[] { "Id", "CountryCode", "IsPrimary", "Number" },
                values: new object[,]
                {
                    { 1, "+90", true, "5559998877" },
                    { 2, "+49", false, "15233445566" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppSettingReservationNumbers_CountryCode_Number",
                table: "AppSettingReservationNumbers",
                columns: new[] { "CountryCode", "Number" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppSettingWhatsappLines_CountryCode_Number",
                table: "AppSettingWhatsappLines",
                columns: new[] { "CountryCode", "Number" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppSettingReservationLinks");

            migrationBuilder.DropTable(
                name: "AppSettingReservationNumbers");

            migrationBuilder.DropTable(
                name: "AppSettingWhatsappLines");
        }
    }
}
