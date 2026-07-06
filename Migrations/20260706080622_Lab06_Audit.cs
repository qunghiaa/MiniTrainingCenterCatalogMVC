using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniTrainingCenterCatalog.Mvc.Migrations
{
    /// <inheritdoc />
    public partial class Lab06_Audit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "AuditLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "AuditLogs");
        }
    }
}
