using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APTXHub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Update_Notification_Constraint_Users_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FromUserId",
                table: "Notifications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_FromUserId",
                table: "Notifications",
                column: "FromUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Users_FromUserId",
                table: "Notifications",
                column: "FromUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Users_FromUserId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_FromUserId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "FromUserId",
                table: "Notifications");
        }
    }
}
