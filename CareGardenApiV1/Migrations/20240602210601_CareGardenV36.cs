using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareGardenApiV1.Migrations
{
    /// <inheritdoc />
    public partial class CareGardenV36 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "appointmentId",
                table: "Comment",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "aspectsOfPoint",
                table: "Comment",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "aspectsOfWorkerPoint",
                table: "Comment",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isShowProfile",
                table: "Comment",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid?[]>(
                name: "workerIds",
                table: "Comment",
                type: "uuid[]",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "workerPoint",
                table: "Comment",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "Appointment",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "cancellationDescription",
                table: "Appointment",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comment_appointmentId",
                table: "Comment",
                column: "appointmentId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Appointment_appointmentId",
                table: "Comment",
                column: "appointmentId",
                principalTable: "Appointment",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Appointment_appointmentId",
                table: "Comment");

            migrationBuilder.DropIndex(
                name: "IX_Comment_appointmentId",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "appointmentId",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "aspectsOfPoint",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "aspectsOfWorkerPoint",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "isShowProfile",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "workerIds",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "workerPoint",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "cancellationDescription",
                table: "Appointment");

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "Appointment",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldNullable: true);
        }
    }
}
