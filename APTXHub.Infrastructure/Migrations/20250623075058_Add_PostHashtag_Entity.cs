using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APTXHub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_PostHashtag_Entity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PostHashtags",
                columns: table => new
                {
                    PostId = table.Column<int>(type: "int", nullable: false),
                    HashtagId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostHashtags", x => new { x.PostId, x.HashtagId });
                    table.ForeignKey(
                        name: "FK_PostHashtags_Hashtags_HashtagId",
                        column: x => x.HashtagId,
                        principalTable: "Hashtags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostHashtags_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PostHashtags_HashtagId",
                table: "PostHashtags",
                column: "HashtagId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostHashtags");
        }
    }
}
