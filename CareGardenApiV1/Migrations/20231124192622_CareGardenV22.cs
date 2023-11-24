using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareGardenApiV1.Migrations
{
    /// <inheritdoc />
    public partial class CareGardenV22 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isPopular",
                table: "BusinessService",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Discount",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    businessId = table.Column<Guid>(type: "uuid", nullable: true),
                    serviceIds = table.Column<string>(type: "text", nullable: false),
                    isActive = table.Column<bool>(type: "boolean", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    descriptionEn = table.Column<string>(type: "text", nullable: false),
                    discountRate = table.Column<double>(type: "double precision", nullable: false),
                    discountType = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discount", x => x.id);
                    table.ForeignKey(
                        name: "FK_Discount_Business_businessId",
                        column: x => x.businessId,
                        principalTable: "Business",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Discount_businessId_isActive",
                table: "Discount",
                columns: new[] { "businessId", "isActive" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Discount");

            migrationBuilder.DropColumn(
                name: "isPopular",
                table: "BusinessService");
        }
    }
}
