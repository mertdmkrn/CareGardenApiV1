using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace CareGardenApiV1.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AlterDatabase()
            //    .Annotation("Npgsql:PostgresExtension:postgis", ",,");

            //migrationBuilder.CreateTable(
            //    name: "Business",
            //    columns: table => new
            //    {
            //        id = table.Column<Guid>(type: "uuid", nullable: false),
            //        name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
            //        nameForUrl = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
            //        description = table.Column<string>(type: "text", nullable: true),
            //        descriptionEn = table.Column<string>(type: "text", nullable: true),
            //        city = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
            //        province = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
            //        district = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
            //        address = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
            //        telephone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
            //        email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
            //        latitude = table.Column<double>(type: "double precision", nullable: false),
            //        longitude = table.Column<double>(type: "double precision", nullable: false),
            //        location = table.Column<Point>(type: "geometry (point)", nullable: true),
            //        createDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
            //        updateDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
            //        workingGenderType = table.Column<int>(type: "integer", nullable: false),
            //        workingSizeType = table.Column<int>(type: "integer", nullable: false),
            //        serviceIds = table.Column<string>(type: "text", nullable: true),
            //        officialHolidayAvailable = table.Column<bool>(type: "boolean", nullable: false),
            //        appointmentTimeInterval = table.Column<int>(type: "integer", nullable: false),
            //        appointmentPeopleCount = table.Column<int>(type: "integer", nullable: false),
            //        isActive = table.Column<bool>(type: "boolean", nullable: false),
            //        verified = table.Column<bool>(type: "boolean", nullable: false),
            //        isFeatured = table.Column<bool>(type: "boolean", nullable: false),
            //        hasPromotion = table.Column<bool>(type: "boolean", nullable: false),
            //        logoUrl = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
            //        hasNotification = table.Column<bool>(type: "boolean", nullable: false),
            //        mobileOrOnlineServiceOnly = table.Column<bool>(type: "boolean", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Business", x => x.id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "ConfirmationInfo",
            //    columns: table => new
            //    {
            //        id = table.Column<Guid>(type: "uuid", nullable: false),
            //        target = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
            //        code = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
            //        createDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_ConfirmationInfo", x => x.id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Faq",
            //    columns: table => new
            //    {
            //        id = table.Column<Guid>(type: "uuid", nullable: false),
            //        question = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
            //        questionEn = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
            //        answer = table.Column<string>(type: "text", nullable: true),
            //        answerEn = table.Column<string>(type: "text", nullable: true),
            //        category = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
            //        categoryEn = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
            //        sortOrder = table.Column<int>(type: "integer", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Faq", x => x.id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "ResetLink",
            //    columns: table => new
            //    {
            //        id = table.Column<Guid>(type: "uuid", nullable: false),
            //        email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
            //        linkId = table.Column<Guid>(type: "uuid", maxLength: 100, nullable: true),
            //        createDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_ResetLink", x => x.id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Services",
            //    columns: table => new
            //    {
            //        id = table.Column<Guid>(type: "uuid", nullable: false),
            //        name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
            //        nameEn = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
            //        className = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
            //        colorCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
            //        sortOrder = table.Column<int>(type: "integer", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Services", x => x.id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Setting",
            //    columns: table => new
            //    {
            //        id = table.Column<Guid>(type: "uuid", nullable: false),
            //        name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
            //        description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
            //        type = table.Column<int>(type: "integer", nullable: false),
            //        value = table.Column<string>(type: "text", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Setting", x => x.id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "User",
            //    columns: table => new
            //    {
            //        id = table.Column<Guid>(type: "uuid", nullable: false),
            //        fullName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
            //        gender = table.Column<int>(type: "integer", nullable: false),
            //        email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
            //        telephone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
            //        password = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
            //        city = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
            //        createDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
            //        updateDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
            //        birthDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
            //        services = table.Column<string>(type: "text", nullable: true),
            //        role = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
            //        imageUrl = table.Column<string>(type: "text", nullable: true),
            //        isBan = table.Column<bool>(type: "boolean", nullable: false),
            //        latitude = table.Column<double>(type: "double precision", nullable: true),
            //        longitude = table.Column<double>(type: "double precision", nullable: true),
            //        location = table.Column<Point>(type: "geometry (point)", nullable: true),
            //        hasNotification = table.Column<bool>(type: "boolean", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_User", x => x.id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "BusinessGallery",
            //    columns: table => new
            //    {
            //        id = table.Column<Guid>(type: "uuid", nullable: false),
            //        imageUrl = table.Column<string>(type: "text", nullable: true),
            //        size = table.Column<string>(type: "character varying(9)", maxLength: 9, nullable: true),
            //        isProfilePhoto = table.Column<bool>(type: "boolean", nullable: false),
            //        isSliderPhoto = table.Column<bool>(type: "boolean", nullable: false),
            //        sortOrder = table.Column<int>(type: "integer", nullable: false),
            //        businessId = table.Column<Guid>(type: "uuid", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_BusinessGallery", x => x.id);
            //        table.ForeignKey(
            //            name: "FK_BusinessGallery_Business_businessId",
            //            column: x => x.businessId,
            //            principalTable: "Business",
            //            principalColumn: "id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "BusinessProperties",
            //    columns: table => new
            //    {
            //        id = table.Column<Guid>(type: "uuid", nullable: false),
            //        key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
            //        value = table.Column<string>(type: "text", nullable: true),
            //        businessId = table.Column<Guid>(type: "uuid", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_BusinessProperties", x => x.id);
            //        table.ForeignKey(
            //            name: "FK_BusinessProperties_Business_businessId",
            //            column: x => x.businessId,
            //            principalTable: "Business",
            //            principalColumn: "id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "BusinessUser",
            //    columns: table => new
            //    {
            //        id = table.Column<Guid>(type: "uuid", nullable: false),
            //        fullName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
            //        title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
            //        gender = table.Column<int>(type: "integer", nullable: false),
            //        email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
            //        telephone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
            //        password = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
            //        createDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
            //        updateDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
            //        birthDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
            //        role = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
            //        imageUrl = table.Column<string>(type: "text", nullable: true),
            //        isBan = table.Column<bool>(type: "boolean", nullable: false),
            //        hasNotification = table.Column<bool>(type: "boolean", nullable: false),
            //        businessId = table.Column<Guid>(type: "uuid", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_BusinessUser", x => x.id);
            //        table.ForeignKey(
            //            name: "FK_BusinessUsers_Business_businessId",
            //            column: x => x.businessId,
            //            principalTable: "Business",
            //            principalColumn: "id",
            //            onDelete: ReferentialAction.SetNull);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "BusinessWorkingInfo",
            //    columns: table => new
            //    {
            //        id = table.Column<Guid>(type: "uuid", nullable: false),
            //        mondayWorkHours = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
            //        tuesdayWorkHours = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
            //        wednesdayWorkHours = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
            //        thursdayWorkHours = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
            //        fridayWorkHours = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
            //        saturdayWorkHours = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
            //        sundayWorkHours = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
            //        businessId = table.Column<Guid>(type: "uuid", nullable: true),
            //        officialHolidayAvailable = table.Column<bool>(type: "boolean", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_BusinessWorkingInfo", x => x.id);
            //        table.ForeignKey(
            //            name: "FK_BusinessWorkingInfo_Business_businessId",
            //            column: x => x.businessId,
            //            principalTable: "Business",
            //            principalColumn: "id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Campaign",
            //    columns: table => new
            //    {
            //        id = table.Column<Guid>(type: "uuid", nullable: false),
            //        path = table.Column<string>(type: "text", nullable: true),
            //        pathEn = table.Column<string>(type: "text", nullable: true),
            //        url = table.Column<string>(type: "text", nullable: true),
            //        title = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
            //        titleEn = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
            //        about = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
            //        aboutEn = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
            //        condition = table.Column<string>(type: "text", nullable: true),
            //        conditionEn = table.Column<string>(type: "text", nullable: true),
            //        isActive = table.Column<bool>(type: "boolean", nullable: false),
            //        createDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
            //        updateDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
            //        expireDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
            //        sortOrder = table.Column<int>(type: "integer", nullable: false),
            //        businessId = table.Column<Guid>(type: "uuid", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Campaign", x => x.id);
            //        table.ForeignKey(
            //            name: "FK_Campaign_Business_businessId",
            //            column: x => x.businessId,
            //            principalTable: "Business",
            //            principalColumn: "id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Discount",
            //    columns: table => new
            //    {
            //        id = table.Column<Guid>(type: "uuid", nullable: false),
            //        businessId = table.Column<Guid>(type: "uuid", nullable: true),
            //        serviceIds = table.Column<string>(type: "text", nullable: false),
            //        isActive = table.Column<bool>(type: "boolean", nullable: false),
            //        description = table.Column<string>(type: "text", nullable: false),
            //        descriptionEn = table.Column<string>(type: "text", nullable: false),
            //        rate = table.Column<double>(type: "double precision", nullable: false),
            //        type = table.Column<int>(type: "integer", nullable: false),
            //        colorCode = table.Column<string>(type: "text", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Discount", x => x.id);
            //        table.ForeignKey(
            //            name: "FK_Discount_Business_businessId",
            //            column: x => x.businessId,
            //            principalTable: "Business",
            //            principalColumn: "id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "PaymentInfo",
            //    columns: table => new
            //    {
            //        id = table.Column<Guid>(type: "uuid", nullable: false),
            //        description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
            //        date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
            //        payDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
            //        amount = table.Column<double>(type: "double precision", nullable: false),
            //        payAmount = table.Column<double>(type: "double precision", nullable: false),
            //        receiptFilePath = table.Column<string>(type: "text", nullable: true),
            //        paidType = table.Column<int>(type: "integer", nullable: false),
            //        isPaid = table.Column<bool>(type: "boolean", nullable: false),
            //        businessId = table.Column<Guid>(type: "uuid", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_PaymentInfo", x => x.id);
            //        table.ForeignKey(
            //            name: "FK_PaymentInfo_Business_businessId",
            //            column: x => x.businessId,
            //            principalTable: "Business",
            //            principalColumn: "id");
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Worker",
            //    columns: table => new
            //    {
            //        id = table.Column<Guid>(type: "uuid", nullable: false),
            //        name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
            //        title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
            //        titleEn = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
            //        about = table.Column<string>(type: "text", nullable: true),
            //        aboutEn = table.Column<string>(type: "text", nullable: true),
            //        path = table.Column<string>(type: "text", nullable: true),
            //        isActive = table.Column<bool>(type: "boolean", nullable: false),
            //        isAvailable = table.Column<bool>(type: "boolean", nullable: false),
            //        serviceIds = table.Column<string>(type: "text", nullable: false),
            //        mondayWorkHours = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
            //        tuesdayWorkHours = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
            //        wednesdayWorkHours = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
            //        thursdayWorkHours = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
            //        fridayWorkHours = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
            //        saturdayWorkHours = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
            //        sundayWorkHours = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
            //        businessId = table.Column<Guid>(type: "uuid", nullable: true),
            //        createdUserId = table.Column<Guid>(type: "uuid", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Worker", x => x.id);
            //        table.ForeignKey(
            //            name: "FK_Worker_Business_businessId",
            //            column: x => x.businessId,
            //            principalTable: "Business",
            //            principalColumn: "id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "BusinessService",
            //    columns: table => new
            //    {
            //        id = table.Column<Guid>(type: "uuid", nullable: false),
            //        businessId = table.Column<Guid>(type: "uuid", nullable: true),
            //        serviceId = table.Column<Guid>(type: "uuid", nullable: true),
            //        name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
            //        nameEn = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
            //        spot = table.Column<string>(type: "text", nullable: true),
            //        spotEn = table.Column<string>(type: "text", nullable: true),
            //        minDuration = table.Column<int>(type: "integer", nullable: false),
            //        maxDuration = table.Column<int>(type: "integer", nullable: false),
            //        price = table.Column<double>(type: "double precision", nullable: false),
            //        isPopular = table.Column<bool>(type: "boolean", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_BusinessService", x => x.id);
            //        table.ForeignKey(
            //            name: "FK_BusinessService_Business_businessId",
            //            column: x => x.businessId,
            //            principalTable: "Business",
            //            principalColumn: "id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_BusinessService_Services_serviceId",
            //            column: x => x.serviceId,
            //            principalTable: "Services",
            //            principalColumn: "id",
            //            onDelete: ReferentialAction.SetNull);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Appointment",
            //    columns: table => new
            //    {
            //        id = table.Column<Guid>(type: "uuid", nullable: false),
            //        startDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
            //        endDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
            //        createDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
            //        updateDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
            //        userTelephone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
            //        userName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
            //        userEmail = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
            //        description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
            //        status = table.Column<int>(type: "integer", nullable: false),
            //        userId = table.Column<Guid>(type: "uuid", nullable: true),
            //        businessId = table.Column<Guid>(type: "uuid", nullable: true),
            //        totalPrice = table.Column<double>(type: "double precision", nullable: false),
            //        totalDiscountPrice = table.Column<double>(type: "double precision", nullable: false),
            //        isGuest = table.Column<bool>(type: "boolean", nullable: false),
            //        cancellationDescription = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Appointment", x => x.id);
            //        table.ForeignKey(
            //            name: "FK_Appointment_Business_businessId",
            //            column: x => x.businessId,
            //            principalTable: "Business",
            //            principalColumn: "id",
            //            onDelete: ReferentialAction.SetNull);
            //        table.ForeignKey(
            //            name: "FK_Appointment_User_userId",
            //            column: x => x.userId,
            //            principalTable: "User",
            //            principalColumn: "id",
            //            onDelete: ReferentialAction.SetNull);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Complain",
            //    columns: table => new
            //    {
            //        id = table.Column<Guid>(type: "uuid", nullable: false),
            //        date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
            //        description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
            //        userId = table.Column<Guid>(type: "uuid", nullable: true),
            //        businessId = table.Column<Guid>(type: "uuid", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Complain", x => x.id);
            //        table.ForeignKey(
            //            name: "FK_Complain_Business_businessId",
            //            column: x => x.businessId,
            //            principalTable: "Business",
            //            principalColumn: "id",
            //            onDelete: ReferentialAction.SetNull);
            //        table.ForeignKey(
            //            name: "FK_Complain_User_userId",
            //            column: x => x.userId,
            //            principalTable: "User",
            //            principalColumn: "id",
            //            onDelete: ReferentialAction.SetNull);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Favorite",
            //    columns: table => new
            //    {
            //        id = table.Column<Guid>(type: "uuid", nullable: false),
            //        userId = table.Column<Guid>(type: "uuid", nullable: true),
            //        businessId = table.Column<Guid>(type: "uuid", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Favorite", x => x.id);
            //        table.ForeignKey(
            //            name: "FK_Favorite_Business_businessId",
            //            column: x => x.businessId,
            //            principalTable: "Business",
            //            principalColumn: "id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_Favorite_User_userId",
            //            column: x => x.userId,
            //            principalTable: "User",
            //            principalColumn: "id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Notification",
            //    columns: table => new
            //    {
            //        id = table.Column<Guid>(type: "uuid", nullable: false),
            //        publishDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
            //        title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
            //        titleEn = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
            //        description = table.Column<string>(type: "text", nullable: true),
            //        descriptionEn = table.Column<string>(type: "text", nullable: true),
            //        type = table.Column<int>(type: "integer", nullable: false),
            //        redirectId = table.Column<Guid>(type: "uuid", nullable: true),
            //        redirectUrl = table.Column<string>(type: "text", nullable: true),
            //        isRead = table.Column<bool>(type: "boolean", nullable: false),
            //        createDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
            //        updateDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
            //        userId = table.Column<Guid>(type: "uuid", nullable: true),
            //        businessId = table.Column<Guid>(type: "uuid", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Notification", x => x.id);
            //        table.ForeignKey(
            //            name: "FK_Notification_Business_businessId",
            //            column: x => x.businessId,
            //            principalTable: "Business",
            //            principalColumn: "id",
            //            onDelete: ReferentialAction.SetNull);
            //        table.ForeignKey(
            //            name: "FK_Notification_User_userId",
            //            column: x => x.userId,
            //            principalTable: "User",
            //            principalColumn: "id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "WorkerServicePrice",
            //    columns: table => new
            //    {
            //        id = table.Column<Guid>(type: "uuid", nullable: false),
            //        businessServiceId = table.Column<Guid>(type: "uuid", nullable: true),
            //        workerId = table.Column<Guid>(type: "uuid", nullable: true),
            //        price = table.Column<double>(type: "double precision", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_WorkerServicePrice", x => x.id);
            //        table.ForeignKey(
            //            name: "FK_WorkerServicePrices_BusinessService_businessServiceId",
            //            column: x => x.businessServiceId,
            //            principalTable: "BusinessService",
            //            principalColumn: "id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_WorkerServicePrices_Worker_workerId",
            //            column: x => x.workerId,
            //            principalTable: "Worker",
            //            principalColumn: "id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "AppointmentDetail",
            //    columns: table => new
            //    {
            //        id = table.Column<Guid>(type: "uuid", nullable: false),
            //        appointmentId = table.Column<Guid>(type: "uuid", nullable: true),
            //        workerId = table.Column<Guid>(type: "uuid", nullable: true),
            //        businessServiceId = table.Column<Guid>(type: "uuid", nullable: true),
            //        price = table.Column<double>(type: "double precision", nullable: false),
            //        discountPrice = table.Column<double>(type: "double precision", nullable: false),
            //        date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_AppointmentDetail", x => x.id);
            //        table.ForeignKey(
            //            name: "FK_AppointmentDetail_Appointment_appointmentId",
            //            column: x => x.appointmentId,
            //            principalTable: "Appointment",
            //            principalColumn: "id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_AppointmentDetail_BusinessService_businessServiceId",
            //            column: x => x.businessServiceId,
            //            principalTable: "BusinessService",
            //            principalColumn: "id",
            //            onDelete: ReferentialAction.SetNull);
            //        table.ForeignKey(
            //            name: "FK_AppointmentDetail_Worker_workerId",
            //            column: x => x.workerId,
            //            principalTable: "Worker",
            //            principalColumn: "id",
            //            onDelete: ReferentialAction.SetNull);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Comment",
            //    columns: table => new
            //    {
            //        id = table.Column<Guid>(type: "uuid", nullable: false),
            //        comment = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
            //        point = table.Column<double>(type: "double precision", nullable: false),
            //        workerPoint = table.Column<double>(type: "double precision", nullable: false),
            //        aspectsOfPoint = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
            //        aspectsOfWorkerPoint = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
            //        createDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
            //        updateDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
            //        userId = table.Column<Guid>(type: "uuid", nullable: true),
            //        businessId = table.Column<Guid>(type: "uuid", nullable: true),
            //        appointmentId = table.Column<Guid>(type: "uuid", nullable: true),
            //        replyId = table.Column<Guid>(type: "uuid", nullable: true),
            //        commentType = table.Column<int>(type: "integer", nullable: false),
            //        isShowProfile = table.Column<bool>(type: "boolean", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Comment", x => x.id);
            //        table.ForeignKey(
            //            name: "FK_Comment_Appointment_appointmentId",
            //            column: x => x.appointmentId,
            //            principalTable: "Appointment",
            //            principalColumn: "id");
            //        table.ForeignKey(
            //            name: "FK_Comment_Business_businessId",
            //            column: x => x.businessId,
            //            principalTable: "Business",
            //            principalColumn: "id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_Comment_Comment_replyId",
            //            column: x => x.replyId,
            //            principalTable: "Comment",
            //            principalColumn: "id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_Comment_User_userId",
            //            column: x => x.userId,
            //            principalTable: "User",
            //            principalColumn: "id",
            //            onDelete: ReferentialAction.SetNull);
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_Appointment_businessId",
            //    table: "Appointment",
            //    column: "businessId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Appointment_startDate",
            //    table: "Appointment",
            //    column: "startDate");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Appointment_status",
            //    table: "Appointment",
            //    column: "status");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Appointment_userId",
            //    table: "Appointment",
            //    column: "userId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_AppointmentDetail_appointmentId",
            //    table: "AppointmentDetail",
            //    column: "appointmentId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_AppointmentDetail_businessServiceId",
            //    table: "AppointmentDetail",
            //    column: "businessServiceId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_AppointmentDetail_workerId",
            //    table: "AppointmentDetail",
            //    column: "workerId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Business_city",
            //    table: "Business",
            //    column: "city");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Business_email",
            //    table: "Business",
            //    column: "email");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Business_nameForUrl",
            //    table: "Business",
            //    column: "nameForUrl");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Business_telephone",
            //    table: "Business",
            //    column: "telephone");

            //migrationBuilder.CreateIndex(
            //    name: "IX_BusinessGallery_businessId",
            //    table: "BusinessGallery",
            //    column: "businessId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_BusinessProperties_businessId",
            //    table: "BusinessProperties",
            //    column: "businessId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_BusinessService_businessId",
            //    table: "BusinessService",
            //    column: "businessId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_BusinessService_serviceId",
            //    table: "BusinessService",
            //    column: "serviceId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_BusinessUser_businessId",
            //    table: "BusinessUser",
            //    column: "businessId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_BusinessUser_email",
            //    table: "BusinessUser",
            //    column: "email");

            //migrationBuilder.CreateIndex(
            //    name: "IX_BusinessUser_telephone",
            //    table: "BusinessUser",
            //    column: "telephone");

            //migrationBuilder.CreateIndex(
            //    name: "IX_BusinessWorkingInfo_businessId",
            //    table: "BusinessWorkingInfo",
            //    column: "businessId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Campaign_businessId",
            //    table: "Campaign",
            //    column: "businessId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Campaign_isActive",
            //    table: "Campaign",
            //    column: "isActive");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Comment_appointmentId",
            //    table: "Comment",
            //    column: "appointmentId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Comment_businessId",
            //    table: "Comment",
            //    column: "businessId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Comment_replyId",
            //    table: "Comment",
            //    column: "replyId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Comment_userId",
            //    table: "Comment",
            //    column: "userId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Complain_businessId",
            //    table: "Complain",
            //    column: "businessId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Complain_userId",
            //    table: "Complain",
            //    column: "userId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_ConfirmationInfo_target",
            //    table: "ConfirmationInfo",
            //    column: "target");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Discount_businessId",
            //    table: "Discount",
            //    column: "businessId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Discount_isActive",
            //    table: "Discount",
            //    column: "isActive");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Favorite_businessId",
            //    table: "Favorite",
            //    column: "businessId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Favorite_userId",
            //    table: "Favorite",
            //    column: "userId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Notification_businessId",
            //    table: "Notification",
            //    column: "businessId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Notification_userId",
            //    table: "Notification",
            //    column: "userId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_PaymentInfo_businessId",
            //    table: "PaymentInfo",
            //    column: "businessId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_PaymentInfo_isPaid",
            //    table: "PaymentInfo",
            //    column: "isPaid");

            //migrationBuilder.CreateIndex(
            //    name: "IX_PaymentInfo_paidType",
            //    table: "PaymentInfo",
            //    column: "paidType");

            //migrationBuilder.CreateIndex(
            //    name: "IX_ResetLink_email",
            //    table: "ResetLink",
            //    column: "email");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Setting_name",
            //    table: "Setting",
            //    column: "name",
            //    unique: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_User_email",
            //    table: "User",
            //    column: "email");

            //migrationBuilder.CreateIndex(
            //    name: "IX_User_telephone",
            //    table: "User",
            //    column: "telephone");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Worker_businessId",
            //    table: "Worker",
            //    column: "businessId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Worker_isActive",
            //    table: "Worker",
            //    column: "isActive");

            //migrationBuilder.CreateIndex(
            //    name: "IX_WorkerServicePrice_businessServiceId",
            //    table: "WorkerServicePrice",
            //    column: "businessServiceId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_WorkerServicePrice_workerId",
            //    table: "WorkerServicePrice",
            //    column: "workerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppointmentDetail");

            migrationBuilder.DropTable(
                name: "BusinessGallery");

            migrationBuilder.DropTable(
                name: "BusinessProperties");

            migrationBuilder.DropTable(
                name: "BusinessUser");

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
