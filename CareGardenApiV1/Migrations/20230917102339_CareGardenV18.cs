using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareGardenApiV1.Migrations
{
    /// <inheritdoc />
    public partial class CareGardenV18 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_Business_businessId",
                table: "Appointment");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_User_userId",
                table: "Appointment");

            migrationBuilder.DropForeignKey(
                name: "FK_BusinessService_Services_serviceId",
                table: "BusinessService");

            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Comment_replyId",
                table: "Comment");

            migrationBuilder.DropForeignKey(
                name: "FK_Comment_User_userId",
                table: "Comment");

            migrationBuilder.DropForeignKey(
                name: "FK_Complain_Business_businessId",
                table: "Complain");

            migrationBuilder.DropForeignKey(
                name: "FK_Complain_User_userId",
                table: "Complain");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentInfo_Business_businessId",
                table: "PaymentInfo");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_Business_businessId",
                table: "Appointment",
                column: "businessId",
                principalTable: "Business",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_User_userId",
                table: "Appointment",
                column: "userId",
                principalTable: "User",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessService_Services_serviceId",
                table: "BusinessService",
                column: "serviceId",
                principalTable: "Services",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Comment_replyId",
                table: "Comment",
                column: "replyId",
                principalTable: "Comment",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_User_userId",
                table: "Comment",
                column: "userId",
                principalTable: "User",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Complain_Business_businessId",
                table: "Complain",
                column: "businessId",
                principalTable: "Business",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Complain_User_userId",
                table: "Complain",
                column: "userId",
                principalTable: "User",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentInfo_Business_businessId",
                table: "PaymentInfo",
                column: "businessId",
                principalTable: "Business",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_Business_businessId",
                table: "Appointment");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_User_userId",
                table: "Appointment");

            migrationBuilder.DropForeignKey(
                name: "FK_BusinessService_Services_serviceId",
                table: "BusinessService");

            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Comment_replyId",
                table: "Comment");

            migrationBuilder.DropForeignKey(
                name: "FK_Comment_User_userId",
                table: "Comment");

            migrationBuilder.DropForeignKey(
                name: "FK_Complain_Business_businessId",
                table: "Complain");

            migrationBuilder.DropForeignKey(
                name: "FK_Complain_User_userId",
                table: "Complain");

            migrationBuilder.DropForeignKey(
                name: "FK_PaymentInfo_Business_businessId",
                table: "PaymentInfo");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_Business_businessId",
                table: "Appointment",
                column: "businessId",
                principalTable: "Business",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_User_userId",
                table: "Appointment",
                column: "userId",
                principalTable: "User",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessService_Services_serviceId",
                table: "BusinessService",
                column: "serviceId",
                principalTable: "Services",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Comment_replyId",
                table: "Comment",
                column: "replyId",
                principalTable: "Comment",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_User_userId",
                table: "Comment",
                column: "userId",
                principalTable: "User",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Complain_Business_businessId",
                table: "Complain",
                column: "businessId",
                principalTable: "Business",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Complain_User_userId",
                table: "Complain",
                column: "userId",
                principalTable: "User",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentInfo_Business_businessId",
                table: "PaymentInfo",
                column: "businessId",
                principalTable: "Business",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
