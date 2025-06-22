﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APTXHub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Update_Report_Reason : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Reason",
                table: "Reports",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Reason",
                table: "Reports");
        }
    }
}
