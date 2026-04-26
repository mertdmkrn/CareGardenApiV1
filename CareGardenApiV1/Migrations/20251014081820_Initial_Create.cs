using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace CareGardenApiV1.Migrations
{
    /// <inheritdoc />
    public partial class Initial_Create : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:postgis", ",,");

            migrationBuilder.CreateTable(
                name: "Business",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    nameForUrl = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    descriptionEn = table.Column<string>(type: "text", nullable: true),
                    city = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    province = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    district = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    address = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    telephone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    latitude = table.Column<double>(type: "double precision", nullable: false),
                    longitude = table.Column<double>(type: "double precision", nullable: false),
                    location = table.Column<Point>(type: "geometry (point)", nullable: true),
                    createDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updateDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    workingGenderType = table.Column<int>(type: "integer", nullable: false),
                    workingSizeType = table.Column<int>(type: "integer", nullable: false),
                    serviceIds = table.Column<string>(type: "text", nullable: true),
                    officialHolidayAvailable = table.Column<bool>(type: "boolean", nullable: false),
                    appointmentTimeInterval = table.Column<int>(type: "integer", nullable: false),
                    appointmentPeopleCount = table.Column<int>(type: "integer", nullable: false),
                    isActive = table.Column<bool>(type: "boolean", nullable: false),
                    verified = table.Column<bool>(type: "boolean", nullable: false),
                    isFeatured = table.Column<bool>(type: "boolean", nullable: false),
                    hasPromotion = table.Column<bool>(type: "boolean", nullable: false),
                    logoUrl = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    hasNotification = table.Column<bool>(type: "boolean", nullable: false),
                    mobileOrOnlineServiceOnly = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Business", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ConfirmationInfo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    target = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    code = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    createDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfirmationInfo", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Faq",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    question = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    questionEn = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    answer = table.Column<string>(type: "text", nullable: true),
                    answerEn = table.Column<string>(type: "text", nullable: true),
                    category = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    categoryEn = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    sortOrder = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Faq", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ResetLink",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    linkId = table.Column<Guid>(type: "uuid", maxLength: 100, nullable: true),
                    createDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResetLink", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    nameEn = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    className = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    colorCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    sortOrder = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Setting",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    type = table.Column<int>(type: "integer", nullable: false),
                    value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Setting", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    fullName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    gender = table.Column<int>(type: "integer", nullable: false),
                    email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    telephone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    password = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    city = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    createDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updateDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    birthDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    services = table.Column<string>(type: "text", nullable: true),
                    role = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    imageUrl = table.Column<string>(type: "text", nullable: true),
                    isBan = table.Column<bool>(type: "boolean", nullable: false),
                    latitude = table.Column<double>(type: "double precision", nullable: true),
                    longitude = table.Column<double>(type: "double precision", nullable: true),
                    openAIRequestCount = table.Column<int>(type: "integer", nullable: false),
                    location = table.Column<Point>(type: "geometry (point)", nullable: true),
                    hasNotification = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "BusinessCustomer",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    fullName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    gender = table.Column<int>(type: "integer", nullable: false),
                    email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    telephone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    imageUrl = table.Column<string>(type: "text", nullable: true),
                    createDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    businessId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessCustomer", x => x.id);
                    table.ForeignKey(
                        name: "FK_BusinessCustomers_Business_businessId",
                        column: x => x.businessId,
                        principalTable: "Business",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BusinessGallery",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    imageUrl = table.Column<string>(type: "text", nullable: true),
                    size = table.Column<string>(type: "character varying(9)", maxLength: 9, nullable: true),
                    isProfilePhoto = table.Column<bool>(type: "boolean", nullable: false),
                    isSliderPhoto = table.Column<bool>(type: "boolean", nullable: false),
                    sortOrder = table.Column<int>(type: "integer", nullable: false),
                    businessId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessGallery", x => x.id);
                    table.ForeignKey(
                        name: "FK_BusinessGallery_Business_businessId",
                        column: x => x.businessId,
                        principalTable: "Business",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
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
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BusinessUser",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    fullName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    gender = table.Column<int>(type: "integer", nullable: false),
                    email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    telephone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    password = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    createDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updateDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    birthDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    role = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    imageUrl = table.Column<string>(type: "text", nullable: true),
                    isBan = table.Column<bool>(type: "boolean", nullable: false),
                    hasNotification = table.Column<bool>(type: "boolean", nullable: false),
                    businessId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessUser", x => x.id);
                    table.ForeignKey(
                        name: "FK_BusinessUsers_Business_businessId",
                        column: x => x.businessId,
                        principalTable: "Business",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "BusinessWorkingInfo",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    mondayWorkHours = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    tuesdayWorkHours = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    wednesdayWorkHours = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    thursdayWorkHours = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    fridayWorkHours = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    saturdayWorkHours = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    sundayWorkHours = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    businessId = table.Column<Guid>(type: "uuid", nullable: true),
                    officialHolidayAvailable = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessWorkingInfo", x => x.id);
                    table.ForeignKey(
                        name: "FK_BusinessWorkingInfo_Business_businessId",
                        column: x => x.businessId,
                        principalTable: "Business",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Campaign",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    path = table.Column<string>(type: "text", nullable: true),
                    pathEn = table.Column<string>(type: "text", nullable: true),
                    url = table.Column<string>(type: "text", nullable: true),
                    title = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    titleEn = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    about = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    aboutEn = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    condition = table.Column<string>(type: "text", nullable: true),
                    conditionEn = table.Column<string>(type: "text", nullable: true),
                    isActive = table.Column<bool>(type: "boolean", nullable: false),
                    createDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updateDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    expireDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    sortOrder = table.Column<int>(type: "integer", nullable: false),
                    businessId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Campaign", x => x.id);
                    table.ForeignKey(
                        name: "FK_Campaign_Business_businessId",
                        column: x => x.businessId,
                        principalTable: "Business",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Discount",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    businessId = table.Column<Guid>(type: "uuid", nullable: true),
                    serviceIds = table.Column<string>(type: "text", nullable: false),
                    isActive = table.Column<bool>(type: "boolean", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    descriptionEn = table.Column<string>(type: "text", nullable: false),
                    rate = table.Column<double>(type: "double precision", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    colorCode = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discount", x => x.id);
                    table.ForeignKey(
                        name: "FK_Discount_Business_businessId",
                        column: x => x.businessId,
                        principalTable: "Business",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.CreateTable(
                name: "Worker",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    titleEn = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    about = table.Column<string>(type: "text", nullable: true),
                    aboutEn = table.Column<string>(type: "text", nullable: true),
                    path = table.Column<string>(type: "text", nullable: true),
                    isActive = table.Column<bool>(type: "boolean", nullable: false),
                    isAvailable = table.Column<bool>(type: "boolean", nullable: false),
                    serviceIds = table.Column<string>(type: "text", nullable: false),
                    mondayWorkHours = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    tuesdayWorkHours = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    wednesdayWorkHours = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    thursdayWorkHours = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    fridayWorkHours = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    saturdayWorkHours = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    sundayWorkHours = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    businessId = table.Column<Guid>(type: "uuid", nullable: true),
                    createdUserId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Worker", x => x.id);
                    table.ForeignKey(
                        name: "FK_Worker_Business_businessId",
                        column: x => x.businessId,
                        principalTable: "Business",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BusinessService",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    businessId = table.Column<Guid>(type: "uuid", nullable: true),
                    serviceId = table.Column<Guid>(type: "uuid", nullable: true),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    nameEn = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    spot = table.Column<string>(type: "text", nullable: true),
                    spotEn = table.Column<string>(type: "text", nullable: true),
                    minDuration = table.Column<int>(type: "integer", nullable: false),
                    maxDuration = table.Column<int>(type: "integer", nullable: false),
                    price = table.Column<double>(type: "double precision", nullable: false),
                    isPopular = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessService", x => x.id);
                    table.ForeignKey(
                        name: "FK_BusinessService_Business_businessId",
                        column: x => x.businessId,
                        principalTable: "Business",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BusinessService_Services_serviceId",
                        column: x => x.serviceId,
                        principalTable: "Services",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Appointment",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    startDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    endDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    createDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updateDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    userTelephone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    userName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    userEmail = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    status = table.Column<int>(type: "integer", nullable: false),
                    userId = table.Column<Guid>(type: "uuid", nullable: true),
                    businessId = table.Column<Guid>(type: "uuid", nullable: true),
                    totalPrice = table.Column<double>(type: "double precision", nullable: false),
                    totalDiscountPrice = table.Column<double>(type: "double precision", nullable: false),
                    isGuest = table.Column<bool>(type: "boolean", nullable: false),
                    cancellationDescription = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointment", x => x.id);
                    table.ForeignKey(
                        name: "FK_Appointment_Business_businessId",
                        column: x => x.businessId,
                        principalTable: "Business",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Appointment_User_userId",
                        column: x => x.userId,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
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
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Complain_User_userId",
                        column: x => x.userId,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
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
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Favorite_User_userId",
                        column: x => x.userId,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notification",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    publishDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    titleEn = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    descriptionEn = table.Column<string>(type: "text", nullable: true),
                    type = table.Column<int>(type: "integer", nullable: false),
                    redirectId = table.Column<Guid>(type: "uuid", nullable: true),
                    redirectUrl = table.Column<string>(type: "text", nullable: true),
                    isRead = table.Column<bool>(type: "boolean", nullable: false),
                    createDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updateDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    userId = table.Column<Guid>(type: "uuid", nullable: true),
                    businessId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.id);
                    table.ForeignKey(
                        name: "FK_Notification_Business_businessId",
                        column: x => x.businessId,
                        principalTable: "Business",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Notification_User_userId",
                        column: x => x.userId,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkerServicePrice",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    businessServiceId = table.Column<Guid>(type: "uuid", nullable: true),
                    workerId = table.Column<Guid>(type: "uuid", nullable: true),
                    price = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkerServicePrice", x => x.id);
                    table.ForeignKey(
                        name: "FK_WorkerServicePrices_BusinessService_businessServiceId",
                        column: x => x.businessServiceId,
                        principalTable: "BusinessService",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkerServicePrices_Worker_workerId",
                        column: x => x.workerId,
                        principalTable: "Worker",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppointmentDetail",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    appointmentId = table.Column<Guid>(type: "uuid", nullable: true),
                    workerId = table.Column<Guid>(type: "uuid", nullable: true),
                    businessServiceId = table.Column<Guid>(type: "uuid", nullable: true),
                    price = table.Column<double>(type: "double precision", nullable: false),
                    discountPrice = table.Column<double>(type: "double precision", nullable: false),
                    date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppointmentDetail", x => x.id);
                    table.ForeignKey(
                        name: "FK_AppointmentDetail_Appointment_appointmentId",
                        column: x => x.appointmentId,
                        principalTable: "Appointment",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppointmentDetail_BusinessService_businessServiceId",
                        column: x => x.businessServiceId,
                        principalTable: "BusinessService",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_AppointmentDetail_Worker_workerId",
                        column: x => x.workerId,
                        principalTable: "Worker",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

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

            migrationBuilder.CreateTable(
                name: "Comment",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    comment = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    point = table.Column<double>(type: "double precision", nullable: false),
                    workerPoint = table.Column<double>(type: "double precision", nullable: false),
                    aspectsOfPoint = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    aspectsOfWorkerPoint = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    createDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    updateDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    userId = table.Column<Guid>(type: "uuid", nullable: true),
                    businessId = table.Column<Guid>(type: "uuid", nullable: true),
                    appointmentId = table.Column<Guid>(type: "uuid", nullable: true),
                    replyId = table.Column<Guid>(type: "uuid", nullable: true),
                    commentType = table.Column<int>(type: "integer", nullable: false),
                    isShowProfile = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comment", x => x.id);
                    table.ForeignKey(
                        name: "FK_Comment_Appointment_appointmentId",
                        column: x => x.appointmentId,
                        principalTable: "Appointment",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Comment_Business_businessId",
                        column: x => x.businessId,
                        principalTable: "Business",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comment_Comment_replyId",
                        column: x => x.replyId,
                        principalTable: "Comment",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comment_User_userId",
                        column: x => x.userId,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.Sql("""
                INSERT INTO "Services" ("id", "name", "nameEn", "className", "colorCode", "sortOrder")
                VALUES
                ('b4306e08-47e9-4992-950c-f2f8249d878a', 'Saç Kesimi', 'Hair Cut', 'haircut', '#91c8f7', 1),
                ('9356f8a9-094e-4c7d-afad-90d1345ab752', 'Sakal', 'Beard', 'beard', '#E0EAFF', 2),
                ('c392b38b-ea25-4494-9aae-350625fa2735', 'Saç Stili', 'Hair Styling', 'hairstyling', '#CCFBEF', 3),
                ('c2343025-15a8-4676-af1f-fb970a309c1b', 'Saç Boyama', 'Hair Coloring', 'haircoloring', '#EEB95D', 4),
                ('a69b3dfa-ba9c-42c3-a09d-42cd28f0bb6a', 'Kaş - Kirpik', 'Eyelash - Eyebrow', 'eyelash', '#CCFBEF', 5),
                ('cdbb94ab-3ccb-4e59-aac3-7272a27e880f', 'Manikür', 'Manicure', 'manicure', '#29e0a3', 6),
                ('4094adc7-93f7-40ea-bdcb-6a7b75539edf', 'Pedikür', 'Pedicure', 'pedicure', '#358541', 7),
                ('4753fd7f-5e76-4d10-982b-5d2bca1b6f1a', 'Oje', 'Nail Varnish', 'oje', '#8c2826', 8),
                ('fc959581-f0de-4bda-926f-0555d9bc877f', 'Cilt Bakımı', 'Skin Care', 'skincare', '#ad8fbd', 9),
                ('430210f7-0fa6-453b-a752-c2fb28d65814', 'Şaç Bakımı', 'Hair Care', 'haircare', '#6ce4c0', 10),
                ('015d8548-ae3c-409b-9690-d7db2a70b4e2', 'Makyaj', 'Make Up', 'makeup', '#84e6b8', 11),
                ('835f2ea7-ce97-410a-a4b2-240d99a93d38', 'Diş', 'Dental', 'dental', '#8191a7', 12),
                ('29bd5ef1-c9e7-4354-bc54-048ac31e4fca', 'Masaj - Spa', 'Message - Spa', 'spa', '#f9da6c', 13),
                ('42903897-54c4-4f89-a9fd-28d6696fc95b', 'Epilasyon', 'Epilation', 'epilation', '#c92c2c', 14);
                """);

            migrationBuilder.Sql("""
                INSERT INTO "User" ("id", "fullName", "gender", "email", "telephone", "password", "city", "createDate", "updateDate", "birthDate", "services", "role", "imageUrl", "isBan", "latitude", "longitude", "location", "hasNotification", "openAIRequestCount")
                VALUES
                ('4ffb3bb4-8390-4a5a-ace1-048b79e4d0e0', 'Tolgahan Özcan', 2, 'ozcantolgahan34@gmail.com', '+905542829066', '73l8gRjwLftklgfdXT+MdiMEjJwGPVMsyVxe16iYpk8=', NULL, '2024-07-22 01:49:12.821401', '2024-07-22 01:49:12.821401', NULL, NULL, 'Admin', NULL, false, NULL, NULL, NULL, false, 0),
                ('44c09c38-9c15-4692-a4f1-3d16f5b536de', 'Mert Can Bakır', 2, 'mertcanbakir94@gmail.com', '+905444018140', '73l8gRjwLftklgfdXT+MdiMEjJwGPVMsyVxe16iYpk8=', NULL, '2024-07-22 01:50:16.134982', '2024-07-22 01:50:16.134982', NULL, NULL, 'Admin', NULL, false, NULL, NULL, NULL, false, 0),
                ('5fe296fa-117c-428d-a34c-ac92081323e6', 'Mert Demirkıran', 2, 'mertdmkrn37@gmail.com', '+905467335939', 'XcNn+wR9BF4hWcMQFQ89eiO5Odqa6Rk2Mbpa0q1weZk=', 'İstanbul', '2024-07-22 00:00:00', '2024-10-02 12:10:10.349177', '1998-08-01 00:00:00', NULL, 'Admin', 'https://iili.io/dnbNgtI.jpg', false, 41.0688945, 28.9927958, ST_GeomFromText('POINT(28.9927958 41.0688945)', 4326), false, 0);
                """);

            migrationBuilder.Sql("""
                INSERT INTO "Campaign" ("id", "path", "pathEn", "url", "title", "titleEn", "about", "aboutEn", "condition", "conditionEn", "isActive", "createDate", "updateDate", "expireDate", "sortOrder", "businessId")
                VALUES
                ('fe90c4c3-e44c-428b-a28a-9b257f1ca6de', 'https://iili.io/dsioxna.jpg', NULL, NULL, 'Kampanya', 'Campaign', 'Kampanya', 'Campaign', 'Kampanya', 'Campaign', true, '2024-09-24 08:51:32.255138', '2024-09-24 08:51:32.255138', NULL, 1, NULL);
                """);

            migrationBuilder.Sql("""
                INSERT INTO "Setting" ("id", "name", "description", "type", "value")
                VALUES
                ('677d6d51-f1d1-4494-9ee8-04243428207e', 'FeedbackSubjects', 'Kullanıcılardan alınacak feedback''lerin konuları.', 1, 'Hata|Bug~Randevu|Appointment~Şikayet|Complain'),
                ('b5cca12c-4a84-4b07-a747-f1991ec0a145', 'TopSearch', 'Aramada en çok aranan kelimeler bölümü.', 1, 'Güzellik Salonu|Beauty Saloon~Estetik|Esthetic~Diş|Dental'),
                ('2cee4c2c-3cda-4c47-86fb-5e4e222fa34d', 'PrivacyPolicy', 'Gizlilik Politikası için html.', 0, 'ŞU 5 VERİYİ İLK İNİTAİL MİGRATİON DA POSTGRE DES EKLEMEK istiyorum nasıl yaparilir yardımcı ol');
                """);

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_businessId",
                table: "Appointment",
                column: "businessId");

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

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentDetail_appointmentId",
                table: "AppointmentDetail",
                column: "appointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentDetail_businessServiceId",
                table: "AppointmentDetail",
                column: "businessServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentDetail_date",
                table: "AppointmentDetail",
                column: "date");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentDetail_id",
                table: "AppointmentDetail",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentDetail_workerId",
                table: "AppointmentDetail",
                column: "workerId");

            migrationBuilder.CreateIndex(
                name: "IX_Business_city",
                table: "Business",
                column: "city");

            migrationBuilder.CreateIndex(
                name: "IX_Business_email",
                table: "Business",
                column: "email");

            migrationBuilder.CreateIndex(
                name: "IX_Business_id",
                table: "Business",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_Business_nameForUrl",
                table: "Business",
                column: "nameForUrl",
                unique: true);

            migrationBuilder.Sql("""
                CREATE UNIQUE INDEX "IX_BusinessProperties_externalSourceUrl_unique"
                ON "BusinessProperties" ("value")
                WHERE "key" = 'externalSourceUrl' AND "value" IS NOT NULL;
                """);

            migrationBuilder.CreateIndex(
                name: "IX_Business_telephone",
                table: "Business",
                column: "telephone");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessCustomer_businessId",
                table: "BusinessCustomer",
                column: "businessId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessGallery_businessId",
                table: "BusinessGallery",
                column: "businessId");

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
                name: "IX_BusinessUser_businessId",
                table: "BusinessUser",
                column: "businessId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessUser_email",
                table: "BusinessUser",
                column: "email");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessUser_telephone",
                table: "BusinessUser",
                column: "telephone");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessWorkingInfo_businessId",
                table: "BusinessWorkingInfo",
                column: "businessId");

            migrationBuilder.CreateIndex(
                name: "IX_Campaign_businessId",
                table: "Campaign",
                column: "businessId");

            migrationBuilder.CreateIndex(
                name: "IX_Campaign_isActive",
                table: "Campaign",
                column: "isActive");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_appointmentId",
                table: "Comment",
                column: "appointmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_businessId",
                table: "Comment",
                column: "businessId");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_commentType",
                table: "Comment",
                column: "commentType");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_replyId",
                table: "Comment",
                column: "replyId");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_userId",
                table: "Comment",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_Complain_businessId",
                table: "Complain",
                column: "businessId");

            migrationBuilder.CreateIndex(
                name: "IX_Complain_userId",
                table: "Complain",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_ConfirmationInfo_target",
                table: "ConfirmationInfo",
                column: "target");

            migrationBuilder.CreateIndex(
                name: "IX_Discount_businessId",
                table: "Discount",
                column: "businessId");

            migrationBuilder.CreateIndex(
                name: "IX_Discount_isActive",
                table: "Discount",
                column: "isActive");

            migrationBuilder.CreateIndex(
                name: "IX_Favorite_businessId",
                table: "Favorite",
                column: "businessId");

            migrationBuilder.CreateIndex(
                name: "IX_Favorite_userId",
                table: "Favorite",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_businessId",
                table: "Notification",
                column: "businessId");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_userId",
                table: "Notification",
                column: "userId");

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
                name: "IX_ResetLink_email",
                table: "ResetLink",
                column: "email");

            migrationBuilder.CreateIndex(
                name: "IX_Setting_name",
                table: "Setting",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_email",
                table: "User",
                column: "email");

            migrationBuilder.CreateIndex(
                name: "IX_User_telephone",
                table: "User",
                column: "telephone");

            migrationBuilder.CreateIndex(
                name: "IX_Worker_businessId",
                table: "Worker",
                column: "businessId");

            migrationBuilder.CreateIndex(
                name: "IX_Worker_isActive",
                table: "Worker",
                column: "isActive");

            migrationBuilder.CreateIndex(
                name: "IX_WorkerServicePrice_businessServiceId",
                table: "WorkerServicePrice",
                column: "businessServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkerServicePrice_workerId",
                table: "WorkerServicePrice",
                column: "workerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppointmentDetail");

            migrationBuilder.Sql("""
                DELETE FROM "Campaign" WHERE "id" = 'fe90c4c3-e44c-428b-a28a-9b257f1ca6de';
                DELETE FROM "User" WHERE "id" IN (
                    '4ffb3bb4-8390-4a5a-ace1-048b79e4d0e0',
                    '44c09c38-9c15-4692-a4f1-3d16f5b536de',
                    '5fe296fa-117c-428d-a34c-ac92081323e6'
                );
                DELETE FROM "Setting" WHERE "id" IN (
                    '677d6d51-f1d1-4494-9ee8-04243428207e',
                    'b5cca12c-4a84-4b07-a747-f1991ec0a145',
                    '2cee4c2c-3cda-4c47-86fb-5e4e222fa34d'
                );
                DELETE FROM "Services" WHERE "id" IN (
                    'b4306e08-47e9-4992-950c-f2f8249d878a',
                    '9356f8a9-094e-4c7d-afad-90d1345ab752',
                    'c392b38b-ea25-4494-9aae-350625fa2735',
                    'c2343025-15a8-4676-af1f-fb970a309c1b',
                    'a69b3dfa-ba9c-42c3-a09d-42cd28f0bb6a',
                    'cdbb94ab-3ccb-4e59-aac3-7272a27e880f',
                    '4094adc7-93f7-40ea-bdcb-6a7b75539edf',
                    '4753fd7f-5e76-4d10-982b-5d2bca1b6f1a',
                    'fc959581-f0de-4bda-926f-0555d9bc877f',
                    '430210f7-0fa6-453b-a752-c2fb28d65814',
                    '015d8548-ae3c-409b-9690-d7db2a70b4e2',
                    '835f2ea7-ce97-410a-a4b2-240d99a93d38',
                    '29bd5ef1-c9e7-4354-bc54-048ac31e4fca',
                    '42903897-54c4-4f89-a9fd-28d6696fc95b'
                );
                """);

            migrationBuilder.Sql("""
                DROP INDEX IF EXISTS "IX_BusinessProperties_externalSourceUrl_unique";
                """);

            migrationBuilder.DropTable(
                name: "BusinessCustomer");

            migrationBuilder.DropTable(
                name: "BusinessGallery");

            migrationBuilder.DropTable(
                name: "BusinessPayment");

            migrationBuilder.DropTable(
                name: "BusinessProperties");

            migrationBuilder.DropTable(
                name: "BusinessWorkingInfo");

            migrationBuilder.DropTable(
                name: "Campaign");

            migrationBuilder.DropTable(
                name: "Comment");

            migrationBuilder.DropTable(
                name: "Complain");

            migrationBuilder.DropTable(
                name: "ConfirmationInfo");

            migrationBuilder.DropTable(
                name: "Discount");

            migrationBuilder.DropTable(
                name: "Faq");

            migrationBuilder.DropTable(
                name: "Favorite");

            migrationBuilder.DropTable(
                name: "Notification");

            migrationBuilder.DropTable(
                name: "PaymentInfo");

            migrationBuilder.DropTable(
                name: "ResetLink");

            migrationBuilder.DropTable(
                name: "Setting");

            migrationBuilder.DropTable(
                name: "WorkerServicePrice");

            migrationBuilder.DropTable(
                name: "BusinessUser");

            migrationBuilder.DropTable(
                name: "Appointment");

            migrationBuilder.DropTable(
                name: "BusinessService");

            migrationBuilder.DropTable(
                name: "Worker");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "Business");
        }
    }
}
