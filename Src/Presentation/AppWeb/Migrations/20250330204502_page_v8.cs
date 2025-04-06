using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppWeb.Migrations
{
    /// <inheritdoc />
    public partial class page_v8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AppSections",
                columns: new[] { "Id", "Content", "IsDeleted", "Name", "SectionType" },
                values: new object[] { 2, "Doğanın nefes kesen güzelliğinin turkuaz sularla buluştuğu Kemer’ in kalbinde konumlanan Türkiz Resort Hotel göz alıcı mimarisi ve sıcak atmosferi ile sizi eşsiz bir mutluluğa davet ediyor.", false, "Imperial Turkiz Resort Hotel", 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AppSections",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
