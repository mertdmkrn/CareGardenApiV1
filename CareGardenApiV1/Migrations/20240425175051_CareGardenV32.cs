using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareGardenApiV1.Migrations
{
    /// <inheritdoc />
    public partial class CareGardenV32 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_BusinessService_businessServiceId",
                table: "Appointment");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_Worker_workerId",
                table: "Appointment");

            migrationBuilder.DropIndex(
                name: "IX_Appointment_businessServiceId",
                table: "Appointment");

            migrationBuilder.DropIndex(
                name: "IX_Appointment_userId_businessId_workerId_startDate_status",
                table: "Appointment");

            migrationBuilder.DropIndex(
                name: "IX_Appointment_workerId",
                table: "Appointment");

            migrationBuilder.DropColumn(
                name: "businessServiceId",
                table: "Appointment");

            migrationBuilder.DropColumn(
                name: "workerId",
                table: "Appointment");

            migrationBuilder.AddColumn<double>(
                name: "totalPrice",
                table: "Appointment",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "AppointmentDetail",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    appointmentId = table.Column<Guid>(type: "uuid", nullable: true),
                    workerId = table.Column<Guid>(type: "uuid", nullable: true),
                    businessServiceId = table.Column<Guid>(type: "uuid", nullable: true),
                    price = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppointmentDetail", x => x.id);
                    table.ForeignKey(
                        name: "FK_AppointmentDetail_Appointment_appointmentId",
                        column: x => x.appointmentId,
                        principalTable: "Appointment",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_AppointmentDetail_BusinessService_businessServiceId",
                        column: x => x.businessServiceId,
                        principalTable: "BusinessService",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_AppointmentDetail_Worker_workerId",
                        column: x => x.workerId,
                        principalTable: "Worker",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_userId_businessId_startDate_status",
                table: "Appointment",
                columns: new[] { "userId", "businessId", "startDate", "status" });

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentDetail_appointmentId_workerId",
                table: "AppointmentDetail",
                columns: new[] { "appointmentId", "workerId" });

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentDetail_businessServiceId",
                table: "AppointmentDetail",
                column: "businessServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentDetail_workerId",
                table: "AppointmentDetail",
                column: "workerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppointmentDetail");

            migrationBuilder.DropIndex(
                name: "IX_Appointment_userId_businessId_startDate_status",
                table: "Appointment");

            migrationBuilder.DropColumn(
                name: "totalPrice",
                table: "Appointment");

            migrationBuilder.AddColumn<Guid>(
                name: "businessServiceId",
                table: "Appointment",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "workerId",
                table: "Appointment",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_businessServiceId",
                table: "Appointment",
                column: "businessServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_userId_businessId_workerId_startDate_status",
                table: "Appointment",
                columns: new[] { "userId", "businessId", "workerId", "startDate", "status" });

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_workerId",
                table: "Appointment",
                column: "workerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_BusinessService_businessServiceId",
                table: "Appointment",
                column: "businessServiceId",
                principalTable: "BusinessService",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_Worker_workerId",
                table: "Appointment",
                column: "workerId",
                principalTable: "Worker",
                principalColumn: "id");
        }
    }
}
