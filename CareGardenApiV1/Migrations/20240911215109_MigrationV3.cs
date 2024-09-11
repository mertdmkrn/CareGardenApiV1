using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareGardenApiV1.Migrations
{
    /// <inheritdoc />
    public partial class MigrationV3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Comment_commentType",
                table: "Comment",
                column: "commentType");

            migrationBuilder.CreateIndex(
                name: "IX_Business_id",
                table: "Business",
                column: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Comment_commentType",
                table: "Comment");

            migrationBuilder.DropIndex(
                name: "IX_Business_id",
                table: "Business");
        }
    }
}
