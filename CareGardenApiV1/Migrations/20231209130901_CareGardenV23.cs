using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareGardenApiV1.Migrations
{
    /// <inheritdoc />
    public partial class CareGardenV23 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "discountRate",
                table: "Business");

            migrationBuilder.RenameColumn(
                name: "discountType",
                table: "Discount",
                newName: "type");

            migrationBuilder.RenameColumn(
                name: "discountRate",
                table: "Discount",
                newName: "rate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "type",
                table: "Discount",
                newName: "discountType");

            migrationBuilder.RenameColumn(
                name: "rate",
                table: "Discount",
                newName: "discountRate");

            migrationBuilder.AddColumn<int>(
                name: "discountRate",
                table: "Business",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
