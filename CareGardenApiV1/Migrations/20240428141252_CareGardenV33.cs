using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareGardenApiV1.Migrations
{
    /// <inheritdoc />
    public partial class CareGardenV33 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "fridayWorkHours",
                table: "Worker",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "mondayWorkHours",
                table: "Worker",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "saturdayWorkHours",
                table: "Worker",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "sundayWorkHours",
                table: "Worker",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "thursdayWorkHours",
                table: "Worker",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "titleEn",
                table: "Worker",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "tuesdayWorkHours",
                table: "Worker",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "wednesdayWorkHours",
                table: "Worker",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "wednesdayWorkHours",
                table: "BusinessWorkingInfo",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "tuesdayWorkHours",
                table: "BusinessWorkingInfo",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "thursdayWorkHours",
                table: "BusinessWorkingInfo",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "sundayWorkHours",
                table: "BusinessWorkingInfo",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "saturdayWorkHours",
                table: "BusinessWorkingInfo",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "mondayWorkHours",
                table: "BusinessWorkingInfo",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "fridayWorkHours",
                table: "BusinessWorkingInfo",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "date",
                table: "AppointmentDetail",
                type: "timestamp without time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "fridayWorkHours",
                table: "Worker");

            migrationBuilder.DropColumn(
                name: "mondayWorkHours",
                table: "Worker");

            migrationBuilder.DropColumn(
                name: "saturdayWorkHours",
                table: "Worker");

            migrationBuilder.DropColumn(
                name: "sundayWorkHours",
                table: "Worker");

            migrationBuilder.DropColumn(
                name: "thursdayWorkHours",
                table: "Worker");

            migrationBuilder.DropColumn(
                name: "titleEn",
                table: "Worker");

            migrationBuilder.DropColumn(
                name: "tuesdayWorkHours",
                table: "Worker");

            migrationBuilder.DropColumn(
                name: "wednesdayWorkHours",
                table: "Worker");

            migrationBuilder.DropColumn(
                name: "date",
                table: "AppointmentDetail");

            migrationBuilder.AlterColumn<string>(
                name: "wednesdayWorkHours",
                table: "BusinessWorkingInfo",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "tuesdayWorkHours",
                table: "BusinessWorkingInfo",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "thursdayWorkHours",
                table: "BusinessWorkingInfo",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "sundayWorkHours",
                table: "BusinessWorkingInfo",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "saturdayWorkHours",
                table: "BusinessWorkingInfo",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "mondayWorkHours",
                table: "BusinessWorkingInfo",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "fridayWorkHours",
                table: "BusinessWorkingInfo",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldNullable: true);
        }
    }
}
