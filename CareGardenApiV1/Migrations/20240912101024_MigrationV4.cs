using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareGardenApiV1.Migrations
{
    /// <inheritdoc />
    public partial class MigrationV4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AppointmentDetail_date",
                table: "AppointmentDetail",
                column: "date");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentDetail_id",
                table: "AppointmentDetail",
                column: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AppointmentDetail_date",
                table: "AppointmentDetail");

            migrationBuilder.DropIndex(
                name: "IX_AppointmentDetail_id",
                table: "AppointmentDetail");
        }
    }
}
