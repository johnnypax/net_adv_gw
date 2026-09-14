using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OfflineServiceDeskEF.Migrations
{
    /// <inheritdoc />
    public partial class RemovedOperator : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "operator",
                table: "tickets");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "operator",
                table: "tickets",
                type: "TEXT",
                nullable: false,
                defaultValue: "N.D.");
        }
    }
}
