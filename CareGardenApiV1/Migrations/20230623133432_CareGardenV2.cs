using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace CareGardenApiV1.Migrations
{
    /// <inheritdoc />
    public partial class CareGardenV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "imageUrl",
                table: "User",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(250)",
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isBan",
                table: "User",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "nameEn",
                table: "Services",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "Services",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "colorCode",
                table: "Services",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "className",
                table: "Services",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.CreateTable(
                name: "Business",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    nameEn = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    descriptionEn = table.Column<string>(type: "text", nullable: true),
                    city = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    province = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    district = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    address = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    telephone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    email = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    password = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    latitude = table.Column<double>(type: "double precision", nullable: false),
                    longitude = table.Column<double>(type: "double precision", nullable: false),
                    location = table.Column<Point>(type: "geometry", nullable: true),
                    createDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updateDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    workingGenderType = table.Column<int>(type: "integer", nullable: false),
                    workingStartHour = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: true),
                    workingEndHour = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: true),
                    appointmentTimeInterval = table.Column<int>(type: "integer", nullable: false),
                    appointmentPeopleCount = table.Column<int>(type: "integer", nullable: false),
                    officialHolidayAvailable = table.Column<bool>(type: "boolean", nullable: false),
                    isActive = table.Column<bool>(type: "boolean", nullable: false),
                    verified = table.Column<bool>(type: "boolean", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Business", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Appointment",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    createDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updateDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<int>(type: "integer", nullable: false),
                    userId = table.Column<Guid>(type: "uuid", nullable: true),
                    businessId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointment", x => x.id);
                    table.ForeignKey(
                        name: "FK_Appointment_Business_businessId",
                        column: x => x.businessId,
                        principalTable: "Business",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Appointment_User_userId",
                        column: x => x.userId,
                        principalTable: "User",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "BusinessGallery",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    imageUrl = table.Column<string>(type: "text", nullable: true),
                    size = table.Column<string>(type: "character varying(9)", maxLength: 9, nullable: true),
                    isProfilePhoto = table.Column<bool>(type: "boolean", nullable: false),
                    businessId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessGallery", x => x.id);
                    table.ForeignKey(
                        name: "FK_BusinessGallery_Business_businessId",
                        column: x => x.businessId,
                        principalTable: "Business",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "BusinessProperties",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    value = table.Column<string>(type: "text", nullable: true),
                    businessId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessProperties", x => x.id);
                    table.ForeignKey(
                        name: "FK_BusinessProperties_Business_businessId",
                        column: x => x.businessId,
                        principalTable: "Business",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "BusinessService",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    businessId = table.Column<Guid>(type: "uuid", nullable: true),
                    serviceId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessService", x => x.id);
                    table.ForeignKey(
                        name: "FK_BusinessService_Business_businessId",
                        column: x => x.businessId,
                        principalTable: "Business",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_BusinessService_Services_serviceId",
                        column: x => x.serviceId,
                        principalTable: "Services",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "BusinessWorkingInfo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    startHour = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: true),
                    endHour = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: true),
                    appointmentTimeInterval = table.Column<int>(type: "integer", nullable: false),
                    appointmentPeopleCount = table.Column<int>(type: "integer", nullable: false),
                    isOffDay = table.Column<bool>(type: "boolean", nullable: false),
                    businessId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessWorkingInfo", x => x.id);
                    table.ForeignKey(
                        name: "FK_BusinessWorkingInfo_Business_businessId",
                        column: x => x.businessId,
                        principalTable: "Business",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Comment",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    comment = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    point = table.Column<double>(type: "double precision", nullable: false),
                    createDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updateDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    userId = table.Column<Guid>(type: "uuid", nullable: true),
                    businessId = table.Column<Guid>(type: "uuid", nullable: true),
                    baseId = table.Column<Guid>(type: "uuid", nullable: true),
                    commentType = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comment", x => x.id);
                    table.ForeignKey(
                        name: "FK_Comment_Business_businessId",
                        column: x => x.businessId,
                        principalTable: "Business",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Comment_User_userId",
                        column: x => x.userId,
                        principalTable: "User",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Complain",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    userId = table.Column<Guid>(type: "uuid", nullable: true),
                    businessId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Complain", x => x.id);
                    table.ForeignKey(
                        name: "FK_Complain_Business_businessId",
                        column: x => x.businessId,
                        principalTable: "Business",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Complain_User_userId",
                        column: x => x.userId,
                        principalTable: "User",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Favorite",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    userId = table.Column<Guid>(type: "uuid", nullable: true),
                    businessId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favorite", x => x.id);
                    table.ForeignKey(
                        name: "FK_Favorite_Business_businessId",
                        column: x => x.businessId,
                        principalTable: "Business",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Favorite_User_userId",
                        column: x => x.userId,
                        principalTable: "User",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "PaymentInfo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    payDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    amount = table.Column<double>(type: "double precision", nullable: false),
                    payAmount = table.Column<double>(type: "double precision", nullable: false),
                    receiptFilePath = table.Column<string>(type: "text", nullable: true),
                    paidType = table.Column<int>(type: "integer", nullable: false),
                    isPaid = table.Column<bool>(type: "boolean", nullable: false),
                    businessId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentInfo", x => x.id);
                    table.ForeignKey(
                        name: "FK_PaymentInfo_Business_businessId",
                        column: x => x.businessId,
                        principalTable: "Business",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_email_telephone",
                table: "User",
                columns: new[] { "email", "telephone" });

            migrationBuilder.CreateIndex(
                name: "IX_ConfirmationInfo_target_code",
                table: "ConfirmationInfo",
                columns: new[] { "target", "code" });

            migrationBuilder.CreateIndex(
                name: "IX_Campaign_businessId",
                table: "Campaign",
                column: "businessId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_businessId",
                table: "Appointment",
                column: "businessId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_userId_businessId_date_status",
                table: "Appointment",
                columns: new[] { "userId", "businessId", "date", "status" });

            migrationBuilder.CreateIndex(
                name: "IX_Business_email_telephone_city",
                table: "Business",
                columns: new[] { "email", "telephone", "city" });

            migrationBuilder.CreateIndex(
                name: "IX_BusinessGallery_businessId",
                table: "BusinessGallery",
                column: "businessId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessProperties_businessId",
                table: "BusinessProperties",
                column: "businessId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessService_businessId",
                table: "BusinessService",
                column: "businessId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessService_serviceId",
                table: "BusinessService",
                column: "serviceId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessWorkingInfo_businessId_date",
                table: "BusinessWorkingInfo",
                columns: new[] { "businessId", "date" });

            migrationBuilder.CreateIndex(
                name: "IX_Comment_businessId",
                table: "Comment",
                column: "businessId");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_userId_businessId",
                table: "Comment",
                columns: new[] { "userId", "businessId" });

            migrationBuilder.CreateIndex(
                name: "IX_Complain_businessId",
                table: "Complain",
                column: "businessId");

            migrationBuilder.CreateIndex(
                name: "IX_Complain_userId_businessId",
                table: "Complain",
                columns: new[] { "userId", "businessId" });

            migrationBuilder.CreateIndex(
                name: "IX_Favorite_businessId",
                table: "Favorite",
                column: "businessId");

            migrationBuilder.CreateIndex(
                name: "IX_Favorite_userId",
                table: "Favorite",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentInfo_businessId_paidType",
                table: "PaymentInfo",
                columns: new[] { "businessId", "paidType" });

            migrationBuilder.AddForeignKey(
                name: "FK_Campaign_Business_businessId",
                table: "Campaign",
                column: "businessId",
                principalTable: "Business",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Campaign_Business_businessId",
                table: "Campaign");

            migrationBuilder.DropTable(
                name: "Appointment");

            migrationBuilder.DropTable(
                name: "BusinessGallery");

            migrationBuilder.DropTable(
                name: "BusinessProperties");

            migrationBuilder.DropTable(
                name: "BusinessService");

            migrationBuilder.DropTable(
                name: "BusinessWorkingInfo");

            migrationBuilder.DropTable(
                name: "Comment");

            migrationBuilder.DropTable(
                name: "Complain");

            migrationBuilder.DropTable(
                name: "Favorite");

            migrationBuilder.DropTable(
                name: "PaymentInfo");

            migrationBuilder.DropTable(
                name: "Business");

            migrationBuilder.DropIndex(
                name: "IX_User_email_telephone",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_ConfirmationInfo_target_code",
                table: "ConfirmationInfo");

            migrationBuilder.DropIndex(
                name: "IX_Campaign_businessId",
                table: "Campaign");

            migrationBuilder.DropColumn(
                name: "isBan",
                table: "User");

            migrationBuilder.AlterColumn<string>(
                name: "imageUrl",
                table: "User",
                type: "character varying(250)",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "nameEn",
                table: "Services",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "Services",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "colorCode",
                table: "Services",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "className",
                table: "Services",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);
        }
    }
}
