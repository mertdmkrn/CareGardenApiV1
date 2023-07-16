using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareGardenApiV1.Migrations
{
    /// <inheritdoc />
    public partial class CareGardenV6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "baseId",
                table: "Comment",
                newName: "replyId");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_replyId",
                table: "Comment",
                column: "replyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Comment_replyId",
                table: "Comment",
                column: "replyId",
                principalTable: "Comment",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Comment_replyId",
                table: "Comment");

            migrationBuilder.DropIndex(
                name: "IX_Comment_replyId",
                table: "Comment");

            migrationBuilder.RenameColumn(
                name: "replyId",
                table: "Comment",
                newName: "baseId");
        }
    }
}
