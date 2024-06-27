using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareGardenApiV1.Migrations
{
    /// <inheritdoc />
    public partial class CareGardenV39 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "about",
                table: "Worker",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "aboutEn",
                table: "Worker",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "about",
                table: "Worker");

            migrationBuilder.DropColumn(
                name: "aboutEn",
                table: "Worker");
        }
    }
}
