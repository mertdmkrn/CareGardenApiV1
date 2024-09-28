using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareGardenApiV1.Migrations
{
    /// <inheritdoc />
    public partial class CareGardenV1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "createDate",
                table: "BusinessCustomer",
                type: "timestamp without time zone",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "imageUrl",
                table: "BusinessCustomer",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "createDate",
                table: "BusinessCustomer");

            migrationBuilder.DropColumn(
                name: "imageUrl",
                table: "BusinessCustomer");
        }
    }
}
