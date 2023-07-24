using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace CareGardenApiV1.Migrations
{
    /// <inheritdoc />
    public partial class CareGardenV9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "latitude",
                table: "User",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<Geometry>(
                name: "location",
                table: "User",
                type: "geometry",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "longitude",
                table: "User",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "latitude",
                table: "User");

            migrationBuilder.DropColumn(
                name: "location",
                table: "User");

            migrationBuilder.DropColumn(
                name: "longitude",
                table: "User");
        }
    }
}
