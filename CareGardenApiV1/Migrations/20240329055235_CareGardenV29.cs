using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareGardenApiV1.Migrations
{
    /// <inheritdoc />
    public partial class CareGardenV29 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "date",
                table: "Notification",
                newName: "updateDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "createDate",
                table: "Notification",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "publishDate",
                table: "Notification",
                type: "timestamp without time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "createDate",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "publishDate",
                table: "Notification");

            migrationBuilder.RenameColumn(
                name: "updateDate",
                table: "Notification",
                newName: "date");
        }
    }
}
