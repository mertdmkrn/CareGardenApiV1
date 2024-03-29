using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareGardenApiV1.Migrations
{
    /// <inheritdoc />
    public partial class CareGardenV10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BusinessWorkingInfo_businessId_date",
                table: "BusinessWorkingInfo");

            migrationBuilder.DropColumn(
                name: "appointmentPeopleCount",
                table: "BusinessWorkingInfo");

            migrationBuilder.DropColumn(
                name: "date",
                table: "BusinessWorkingInfo");

            migrationBuilder.DropColumn(
                name: "workingDayType",
                table: "Business");

            migrationBuilder.DropColumn(
                name: "workingEndHour",
                table: "Business");

            migrationBuilder.DropColumn(
                name: "workingStartHour",
                table: "Business");

            migrationBuilder.RenameColumn(
                name: "appointmentTimeInterval",
                table: "BusinessWorkingInfo",
                newName: "day");

            migrationBuilder.AlterColumn<double>(
                name: "longitude",
                table: "User",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<double>(
                name: "latitude",
                table: "User",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AddColumn<int>(
                name: "maxDuration",
                table: "BusinessService",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "minDuration",
                table: "BusinessService",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "BusinessService",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "price",
                table: "BusinessService",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "spot",
                table: "BusinessService",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BusinessWorkingInfo_businessId_day",
                table: "BusinessWorkingInfo",
                columns: new[] { "businessId", "day" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BusinessWorkingInfo_businessId_day",
                table: "BusinessWorkingInfo");

            migrationBuilder.DropColumn(
                name: "maxDuration",
                table: "BusinessService");

            migrationBuilder.DropColumn(
                name: "minDuration",
                table: "BusinessService");

            migrationBuilder.DropColumn(
                name: "name",
                table: "BusinessService");

            migrationBuilder.DropColumn(
                name: "price",
                table: "BusinessService");

            migrationBuilder.DropColumn(
                name: "spot",
                table: "BusinessService");

            migrationBuilder.RenameColumn(
                name: "day",
                table: "BusinessWorkingInfo",
                newName: "appointmentTimeInterval");

            migrationBuilder.AlterColumn<double>(
                name: "longitude",
                table: "User",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "latitude",
                table: "User",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "appointmentPeopleCount",
                table: "BusinessWorkingInfo",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "date",
                table: "BusinessWorkingInfo",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "workingDayType",
                table: "Business",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "workingEndHour",
                table: "Business",
                type: "character varying(5)",
                maxLength: 5,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "workingStartHour",
                table: "Business",
                type: "character varying(5)",
                maxLength: 5,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BusinessWorkingInfo_businessId_date",
                table: "BusinessWorkingInfo",
                columns: new[] { "businessId", "date" });
        }
    }
}
