using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareGardenApiV1.Migrations
{
    /// <inheritdoc />
    public partial class MigrationV1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BusinessPayment",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    description = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    amount = table.Column<double>(type: "double precision", nullable: false),
                    businessId = table.Column<Guid>(type: "uuid", nullable: true),
                    appointmentId = table.Column<Guid>(type: "uuid", nullable: true),
                    businessUserId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessPayment", x => x.id);
                    table.ForeignKey(
                        name: "FK_BusinessPayment_Appointment_appointmentId",
                        column: x => x.appointmentId,
                        principalTable: "Appointment",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_BusinessPayment_BusinessUser_businessUserId",
                        column: x => x.businessUserId,
                        principalTable: "BusinessUser",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_BusinessPayment_Business_businessId",
                        column: x => x.businessId,
                        principalTable: "Business",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BusinessPayment_appointmentId",
                table: "BusinessPayment",
                column: "appointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessPayment_businessId",
                table: "BusinessPayment",
                column: "businessId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessPayment_businessUserId",
                table: "BusinessPayment",
                column: "businessUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessPayment_date",
                table: "BusinessPayment",
                column: "date");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BusinessPayment");
        }
    }
}
