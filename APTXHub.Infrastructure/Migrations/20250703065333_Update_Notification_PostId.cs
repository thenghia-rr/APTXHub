using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APTXHub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Update_Notification_PostId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PostId",
                table: "Notifications",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PostId",
                table: "Notifications");
        }
    }
}
