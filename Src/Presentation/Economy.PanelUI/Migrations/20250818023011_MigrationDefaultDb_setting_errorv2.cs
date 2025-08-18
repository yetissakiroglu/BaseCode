using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Economy.Panel.UI.Migrations
{
    /// <inheritdoc />
    public partial class MigrationDefaultDb_setting_errorv2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AppAuditLog",
                table: "AppAuditLog");

            migrationBuilder.RenameTable(
                name: "AppAuditLog",
                newName: "AppAuditLogs");

            migrationBuilder.RenameIndex(
                name: "IX_AppAuditLog_UserId",
                table: "AppAuditLogs",
                newName: "IX_AppAuditLogs_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_AppAuditLog_Succeeded",
                table: "AppAuditLogs",
                newName: "IX_AppAuditLogs_Succeeded");

            migrationBuilder.RenameIndex(
                name: "IX_AppAuditLog_CreatedDate",
                table: "AppAuditLogs",
                newName: "IX_AppAuditLogs_CreatedDate");

            migrationBuilder.RenameIndex(
                name: "IX_AppAuditLog_CorrelationId",
                table: "AppAuditLogs",
                newName: "IX_AppAuditLogs_CorrelationId");

            migrationBuilder.RenameIndex(
                name: "IX_AppAuditLog_Action_EntityName",
                table: "AppAuditLogs",
                newName: "IX_AppAuditLogs_Action_EntityName");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppAuditLogs",
                table: "AppAuditLogs",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AppAuditLogs",
                table: "AppAuditLogs");

            migrationBuilder.RenameTable(
                name: "AppAuditLogs",
                newName: "AppAuditLog");

            migrationBuilder.RenameIndex(
                name: "IX_AppAuditLogs_UserId",
                table: "AppAuditLog",
                newName: "IX_AppAuditLog_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_AppAuditLogs_Succeeded",
                table: "AppAuditLog",
                newName: "IX_AppAuditLog_Succeeded");

            migrationBuilder.RenameIndex(
                name: "IX_AppAuditLogs_CreatedDate",
                table: "AppAuditLog",
                newName: "IX_AppAuditLog_CreatedDate");

            migrationBuilder.RenameIndex(
                name: "IX_AppAuditLogs_CorrelationId",
                table: "AppAuditLog",
                newName: "IX_AppAuditLog_CorrelationId");

            migrationBuilder.RenameIndex(
                name: "IX_AppAuditLogs_Action_EntityName",
                table: "AppAuditLog",
                newName: "IX_AppAuditLog_Action_EntityName");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppAuditLog",
                table: "AppAuditLog",
                column: "Id");
        }
    }
}
