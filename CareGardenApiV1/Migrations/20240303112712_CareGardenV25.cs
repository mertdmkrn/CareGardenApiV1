using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareGardenApiV1.Migrations
{
    /// <inheritdoc />
    public partial class CareGardenV25 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Discount_Business_businessId",
                table: "Discount");

            migrationBuilder.AddColumn<string>(
                name: "colorCode",
                table: "Discount",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isSliderPhoto",
                table: "BusinessGallery",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "sortOrder",
                table: "BusinessGallery",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Discount_Business_businessId",
                table: "Discount",
                column: "businessId",
                principalTable: "Business",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Discount_Business_businessId",
                table: "Discount");

            migrationBuilder.DropColumn(
                name: "colorCode",
                table: "Discount");

            migrationBuilder.DropColumn(
                name: "isSliderPhoto",
                table: "BusinessGallery");

            migrationBuilder.DropColumn(
                name: "sortOrder",
                table: "BusinessGallery");

            migrationBuilder.AddForeignKey(
                name: "FK_Discount_Business_businessId",
                table: "Discount",
                column: "businessId",
                principalTable: "Business",
                principalColumn: "id");
        }
    }
}
