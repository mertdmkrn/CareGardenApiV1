using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareGardenApiV1.Migrations
{
    /// <inheritdoc />
    public partial class CareGardenV43 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppointmentDetail_Appointment_appointmentId",
                table: "AppointmentDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_AppointmentDetail_BusinessService_businessServiceId",
                table: "AppointmentDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_AppointmentDetail_Worker_workerId",
                table: "AppointmentDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_Notification_Business_businessId",
                table: "Notification");

            migrationBuilder.DropForeignKey(
                name: "FK_Notification_User_userId",
                table: "Notification");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkerServicePrice_BusinessService_businessServiceId",
                table: "WorkerServicePrice");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkerServicePrice_Worker_workerId",
                table: "WorkerServicePrice");

            migrationBuilder.DropIndex(
                name: "IX_WorkerServicePrice_businessServiceId_workerId",
                table: "WorkerServicePrice");

            migrationBuilder.DropIndex(
                name: "IX_User_email_telephone",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_PaymentInfo_businessId_paidType",
                table: "PaymentInfo");

            migrationBuilder.DropIndex(
                name: "IX_Notification_userId_businessId",
                table: "Notification");

            migrationBuilder.DropIndex(
                name: "IX_Discount_businessId_isActive",
                table: "Discount");

            migrationBuilder.DropIndex(
                name: "IX_ConfirmationInfo_target_code",
                table: "ConfirmationInfo");

            migrationBuilder.DropIndex(
                name: "IX_Complain_userId_businessId",
                table: "Complain");

            migrationBuilder.DropIndex(
                name: "IX_Comment_userId_businessId_appointmentId",
                table: "Comment");

            migrationBuilder.DropIndex(
                name: "IX_Business_email_telephone_city",
                table: "Business");

            migrationBuilder.DropIndex(
                name: "IX_AppointmentDetail_appointmentId_workerId",
                table: "AppointmentDetail");

            migrationBuilder.DropIndex(
                name: "IX_Appointment_userId_businessId_startDate_status",
                table: "Appointment");

            migrationBuilder.CreateIndex(
                name: "IX_WorkerServicePrice_businessServiceId",
                table: "WorkerServicePrice",
                column: "businessServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Worker_isActive",
                table: "Worker",
                column: "isActive");

            migrationBuilder.CreateIndex(
                name: "IX_User_email",
                table: "User",
                column: "email");

            migrationBuilder.CreateIndex(
                name: "IX_User_telephone",
                table: "User",
                column: "telephone");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentInfo_businessId",
                table: "PaymentInfo",
                column: "businessId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentInfo_isPaid",
                table: "PaymentInfo",
                column: "isPaid");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentInfo_paidType",
                table: "PaymentInfo",
                column: "paidType");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_userId",
                table: "Notification",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_Discount_businessId",
                table: "Discount",
                column: "businessId");

            migrationBuilder.CreateIndex(
                name: "IX_Discount_isActive",
                table: "Discount",
                column: "isActive");

            migrationBuilder.CreateIndex(
                name: "IX_ConfirmationInfo_target",
                table: "ConfirmationInfo",
                column: "target");

            migrationBuilder.CreateIndex(
                name: "IX_Complain_userId",
                table: "Complain",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_userId",
                table: "Comment",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_Campaign_isActive",
                table: "Campaign",
                column: "isActive");

            migrationBuilder.CreateIndex(
                name: "IX_Business_city",
                table: "Business",
                column: "city");

            migrationBuilder.CreateIndex(
                name: "IX_Business_email",
                table: "Business",
                column: "email");

            migrationBuilder.CreateIndex(
                name: "IX_Business_telephone",
                table: "Business",
                column: "telephone");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentDetail_appointmentId",
                table: "AppointmentDetail",
                column: "appointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_startDate",
                table: "Appointment",
                column: "startDate");

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_status",
                table: "Appointment",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_userId",
                table: "Appointment",
                column: "userId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppointmentDetail_Appointment_appointmentId",
                table: "AppointmentDetail",
                column: "appointmentId",
                principalTable: "Appointment",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppointmentDetail_BusinessService_businessServiceId",
                table: "AppointmentDetail",
                column: "businessServiceId",
                principalTable: "BusinessService",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_AppointmentDetail_Worker_workerId",
                table: "AppointmentDetail",
                column: "workerId",
                principalTable: "Worker",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_Business_businessId",
                table: "Notification",
                column: "businessId",
                principalTable: "Business",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_User_userId",
                table: "Notification",
                column: "userId",
                principalTable: "User",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkerServicePrices_BusinessService_businessServiceId",
                table: "WorkerServicePrice",
                column: "businessServiceId",
                principalTable: "BusinessService",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkerServicePrices_Worker_workerId",
                table: "WorkerServicePrice",
                column: "workerId",
                principalTable: "Worker",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppointmentDetail_Appointment_appointmentId",
                table: "AppointmentDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_AppointmentDetail_BusinessService_businessServiceId",
                table: "AppointmentDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_AppointmentDetail_Worker_workerId",
                table: "AppointmentDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_Notification_Business_businessId",
                table: "Notification");

            migrationBuilder.DropForeignKey(
                name: "FK_Notification_User_userId",
                table: "Notification");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkerServicePrices_BusinessService_businessServiceId",
                table: "WorkerServicePrice");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkerServicePrices_Worker_workerId",
                table: "WorkerServicePrice");

            migrationBuilder.DropIndex(
                name: "IX_WorkerServicePrice_businessServiceId",
                table: "WorkerServicePrice");

            migrationBuilder.DropIndex(
                name: "IX_Worker_isActive",
                table: "Worker");

            migrationBuilder.DropIndex(
                name: "IX_User_email",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_telephone",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_PaymentInfo_businessId",
                table: "PaymentInfo");

            migrationBuilder.DropIndex(
                name: "IX_PaymentInfo_isPaid",
                table: "PaymentInfo");

            migrationBuilder.DropIndex(
                name: "IX_PaymentInfo_paidType",
                table: "PaymentInfo");

            migrationBuilder.DropIndex(
                name: "IX_Notification_userId",
                table: "Notification");

            migrationBuilder.DropIndex(
                name: "IX_Discount_businessId",
                table: "Discount");

            migrationBuilder.DropIndex(
                name: "IX_Discount_isActive",
                table: "Discount");

            migrationBuilder.DropIndex(
                name: "IX_ConfirmationInfo_target",
                table: "ConfirmationInfo");

            migrationBuilder.DropIndex(
                name: "IX_Complain_userId",
                table: "Complain");

            migrationBuilder.DropIndex(
                name: "IX_Comment_userId",
                table: "Comment");

            migrationBuilder.DropIndex(
                name: "IX_Campaign_isActive",
                table: "Campaign");

            migrationBuilder.DropIndex(
                name: "IX_Business_city",
                table: "Business");

            migrationBuilder.DropIndex(
                name: "IX_Business_email",
                table: "Business");

            migrationBuilder.DropIndex(
                name: "IX_Business_telephone",
                table: "Business");

            migrationBuilder.DropIndex(
                name: "IX_AppointmentDetail_appointmentId",
                table: "AppointmentDetail");

            migrationBuilder.DropIndex(
                name: "IX_Appointment_startDate",
                table: "Appointment");

            migrationBuilder.DropIndex(
                name: "IX_Appointment_status",
                table: "Appointment");

            migrationBuilder.DropIndex(
                name: "IX_Appointment_userId",
                table: "Appointment");

            migrationBuilder.CreateIndex(
                name: "IX_WorkerServicePrice_businessServiceId_workerId",
                table: "WorkerServicePrice",
                columns: new[] { "businessServiceId", "workerId" });

            migrationBuilder.CreateIndex(
                name: "IX_User_email_telephone",
                table: "User",
                columns: new[] { "email", "telephone" });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentInfo_businessId_paidType",
                table: "PaymentInfo",
                columns: new[] { "businessId", "paidType" });

            migrationBuilder.CreateIndex(
                name: "IX_Notification_userId_businessId",
                table: "Notification",
                columns: new[] { "userId", "businessId" });

            migrationBuilder.CreateIndex(
                name: "IX_Discount_businessId_isActive",
                table: "Discount",
                columns: new[] { "businessId", "isActive" });

            migrationBuilder.CreateIndex(
                name: "IX_ConfirmationInfo_target_code",
                table: "ConfirmationInfo",
                columns: new[] { "target", "code" });

            migrationBuilder.CreateIndex(
                name: "IX_Complain_userId_businessId",
                table: "Complain",
                columns: new[] { "userId", "businessId" });

            migrationBuilder.CreateIndex(
                name: "IX_Comment_userId_businessId_appointmentId",
                table: "Comment",
                columns: new[] { "userId", "businessId", "appointmentId" });

            migrationBuilder.CreateIndex(
                name: "IX_Business_email_telephone_city",
                table: "Business",
                columns: new[] { "email", "telephone", "city" });

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentDetail_appointmentId_workerId",
                table: "AppointmentDetail",
                columns: new[] { "appointmentId", "workerId" });

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_userId_businessId_startDate_status",
                table: "Appointment",
                columns: new[] { "userId", "businessId", "startDate", "status" });

            migrationBuilder.AddForeignKey(
                name: "FK_AppointmentDetail_Appointment_appointmentId",
                table: "AppointmentDetail",
                column: "appointmentId",
                principalTable: "Appointment",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppointmentDetail_BusinessService_businessServiceId",
                table: "AppointmentDetail",
                column: "businessServiceId",
                principalTable: "BusinessService",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppointmentDetail_Worker_workerId",
                table: "AppointmentDetail",
                column: "workerId",
                principalTable: "Worker",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_Business_businessId",
                table: "Notification",
                column: "businessId",
                principalTable: "Business",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_User_userId",
                table: "Notification",
                column: "userId",
                principalTable: "User",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkerServicePrice_BusinessService_businessServiceId",
                table: "WorkerServicePrice",
                column: "businessServiceId",
                principalTable: "BusinessService",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkerServicePrice_Worker_workerId",
                table: "WorkerServicePrice",
                column: "workerId",
                principalTable: "Worker",
                principalColumn: "id");
        }
    }
}
