using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareGardenApiV1.Migrations
{
    /// <inheritdoc />
    public partial class CareGardenV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "password",
                table: "Business");

            migrationBuilder.AddColumn<bool>(
                name: "mobileOrOnlineServiceOnly",
                table: "Business",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "serviceIds",
                table: "Business",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "workingSizeType",
                table: "Business",
                type: "integer",
                nullable: false,
                defaultValue: 0);

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
                name: "IX_ResetLink_email",
                table: "ResetLink",
                column: "email");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BusinessUser");

            migrationBuilder.DropTable(
                name: "ResetLink");

            migrationBuilder.DropColumn(
                name: "mobileOrOnlineServiceOnly",
                table: "Business");

            migrationBuilder.DropColumn(
                name: "serviceIds",
                table: "Business");

            migrationBuilder.DropColumn(
                name: "workingSizeType",
                table: "Business");

            migrationBuilder.AddColumn<string>(
                name: "password",
                table: "Business",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);
        }
    }
}
