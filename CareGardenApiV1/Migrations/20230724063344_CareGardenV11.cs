using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace CareGardenApiV1.Migrations
{
    /// <inheritdoc />
    public partial class CareGardenV11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Point>(
                name: "location",
                table: "User",
                type: "geometry (point)",
                nullable: true,
                oldClrType: typeof(Point),
                oldType: "geometry",
                oldNullable: true);

            migrationBuilder.AlterColumn<Point>(
                name: "location",
                table: "Business",
                type: "geometry (point)",
                nullable: true,
                oldClrType: typeof(Point),
                oldType: "geometry",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Point>(
                name: "location",
                table: "User",
                type: "geometry",
                nullable: true,
                oldClrType: typeof(Point),
                oldType: "geometry (point)",
                oldNullable: true);

            migrationBuilder.AlterColumn<Point>(
                name: "location",
                table: "Business",
                type: "geometry",
                nullable: true,
                oldClrType: typeof(Point),
                oldType: "geometry (point)",
                oldNullable: true);
        }
    }
}
