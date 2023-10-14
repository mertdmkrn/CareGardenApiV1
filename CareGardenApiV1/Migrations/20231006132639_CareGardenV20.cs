using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareGardenApiV1.Migrations
{
    /// <inheritdoc />
    public partial class CareGardenV20 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Appointment_userId_businessId_date_status",
                table: "Appointment");

            migrationBuilder.RenameColumn(
                name: "date",
                table: "Appointment",
                newName: "startDate");

            migrationBuilder.AddColumn<Guid>(
                name: "businessServiceId",
                table: "Appointment",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "endDate",
                table: "Appointment",
                type: "timestamp without time zone",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "endDate",
                table: "Appointment");

            migrationBuilder.DropColumn(
                name: "workerId",
                table: "Appointment");

            migrationBuilder.RenameColumn(
                name: "startDate",
                table: "Appointment",
                newName: "date");

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_userId_businessId_date_status",
                table: "Appointment",
                columns: new[] { "userId", "businessId", "date", "status" });
        }
    }
}
