using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareGardenApiV1.Migrations
{
    /// <inheritdoc />
    public partial class CareGardenV38 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Comment_appointmentId",
                table: "Comment");

            migrationBuilder.DropIndex(
                name: "IX_Comment_userId_businessId",
                table: "Comment");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_appointmentId",
                table: "Comment",
                column: "appointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_userId_businessId_appointmentId",
                table: "Comment",
                columns: new[] { "userId", "businessId", "appointmentId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Comment_appointmentId",
                table: "Comment");

            migrationBuilder.DropIndex(
                name: "IX_Comment_userId_businessId_appointmentId",
                table: "Comment");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_appointmentId",
                table: "Comment",
                column: "appointmentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comment_userId_businessId",
                table: "Comment",
                columns: new[] { "userId", "businessId" });
        }
    }
}
