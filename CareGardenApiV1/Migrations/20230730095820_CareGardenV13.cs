using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareGardenApiV1.Migrations
{
    /// <inheritdoc />
    public partial class CareGardenV13 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BusinessWorkingInfo_businessId_day",
                table: "BusinessWorkingInfo");

            migrationBuilder.DropColumn(
                name: "day",
                table: "BusinessWorkingInfo");

            migrationBuilder.DropColumn(
                name: "endHour",
                table: "BusinessWorkingInfo");

            migrationBuilder.DropColumn(
                name: "isOffDay",
                table: "BusinessWorkingInfo");

            migrationBuilder.DropColumn(
                name: "startHour",
                table: "BusinessWorkingInfo");

            migrationBuilder.AddColumn<string>(
                name: "fridayWorkHours",
                table: "BusinessWorkingInfo",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "mondayWorkHours",
                table: "BusinessWorkingInfo",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "saturdayWorkHours",
                table: "BusinessWorkingInfo",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "sundayWorkHours",
                table: "BusinessWorkingInfo",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "thursdayWorkHours",
                table: "BusinessWorkingInfo",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "tuesdayWorkHours",
                table: "BusinessWorkingInfo",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "wednesdayWorkHours",
                table: "BusinessWorkingInfo",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessWorkingInfo_businessId",
                table: "BusinessWorkingInfo",
                column: "businessId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BusinessWorkingInfo_businessId",
                table: "BusinessWorkingInfo");

            migrationBuilder.DropColumn(
                name: "fridayWorkHours",
                table: "BusinessWorkingInfo");

            migrationBuilder.DropColumn(
                name: "mondayWorkHours",
                table: "BusinessWorkingInfo");

            migrationBuilder.DropColumn(
                name: "saturdayWorkHours",
                table: "BusinessWorkingInfo");

            migrationBuilder.DropColumn(
                name: "sundayWorkHours",
                table: "BusinessWorkingInfo");

            migrationBuilder.DropColumn(
                name: "thursdayWorkHours",
                table: "BusinessWorkingInfo");

            migrationBuilder.DropColumn(
                name: "tuesdayWorkHours",
                table: "BusinessWorkingInfo");

            migrationBuilder.DropColumn(
                name: "wednesdayWorkHours",
                table: "BusinessWorkingInfo");

            migrationBuilder.AddColumn<int>(
                name: "day",
                table: "BusinessWorkingInfo",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "endHour",
                table: "BusinessWorkingInfo",
                type: "character varying(5)",
                maxLength: 5,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isOffDay",
                table: "BusinessWorkingInfo",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "startHour",
                table: "BusinessWorkingInfo",
                type: "character varying(5)",
                maxLength: 5,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BusinessWorkingInfo_businessId_day",
                table: "BusinessWorkingInfo",
                columns: new[] { "businessId", "day" });
        }
    }
}
