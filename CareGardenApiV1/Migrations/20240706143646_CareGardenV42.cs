using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareGardenApiV1.Migrations
{
    /// <inheritdoc />
    public partial class CareGardenV42 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "Business",
                type: "character varying(150)",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "nameForUrl",
                table: "Business",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Business_nameForUrl",
                table: "Business",
                column: "nameForUrl");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Business_nameForUrl",
                table: "Business");

            migrationBuilder.DropColumn(
                name: "nameForUrl",
                table: "Business");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "Business",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(150)",
                oldMaxLength: 150,
                oldNullable: true);
        }
    }
}
