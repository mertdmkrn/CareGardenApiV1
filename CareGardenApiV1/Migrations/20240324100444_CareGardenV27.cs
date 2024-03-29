using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareGardenApiV1.Migrations
{
    /// <inheritdoc />
    public partial class CareGardenV27 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "about",
                table: "Campaign",
                type: "character varying(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "aboutEn",
                table: "Campaign",
                type: "character varying(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "condition",
                table: "Campaign",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "conditionEn",
                table: "Campaign",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "expireDate",
                table: "Campaign",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "title",
                table: "Campaign",
                type: "character varying(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "titleEn",
                table: "Campaign",
                type: "character varying(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Notification",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    descriptionEn = table.Column<string>(type: "text", nullable: true),
                    type = table.Column<int>(type: "integer", nullable: false),
                    redirectId = table.Column<Guid>(type: "uuid", nullable: true),
                    redirectUrl = table.Column<string>(type: "text", nullable: true),
                    isRead = table.Column<bool>(type: "boolean", nullable: false),
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
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Notification_User_userId",
                        column: x => x.userId,
                        principalTable: "User",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notification_businessId",
                table: "Notification",
                column: "businessId");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_userId_businessId",
                table: "Notification",
                columns: new[] { "userId", "businessId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notification");

            migrationBuilder.DropColumn(
                name: "about",
                table: "Campaign");

            migrationBuilder.DropColumn(
                name: "aboutEn",
                table: "Campaign");

            migrationBuilder.DropColumn(
                name: "condition",
                table: "Campaign");

            migrationBuilder.DropColumn(
                name: "conditionEn",
                table: "Campaign");

            migrationBuilder.DropColumn(
                name: "expireDate",
                table: "Campaign");

            migrationBuilder.DropColumn(
                name: "title",
                table: "Campaign");

            migrationBuilder.DropColumn(
                name: "titleEn",
                table: "Campaign");
        }
    }
}
