using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareGardenApiV1.Migrations
{
    /// <inheritdoc />
    public partial class CareGardenV19 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Comment_replyId",
                table: "Comment");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Comment_replyId",
                table: "Comment",
                column: "replyId",
                principalTable: "Comment",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Comment_replyId",
                table: "Comment");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Comment_replyId",
                table: "Comment",
                column: "replyId",
                principalTable: "Comment",
                principalColumn: "id");
        }
    }
}
