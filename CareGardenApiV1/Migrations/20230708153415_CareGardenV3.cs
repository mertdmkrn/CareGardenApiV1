using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareGardenApiV1.Migrations
{
    /// <inheritdoc />
    public partial class CareGardenV3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BusinessGallery_Business_businessId",
                table: "BusinessGallery");

            migrationBuilder.DropForeignKey(
                name: "FK_BusinessProperties_Business_businessId",
                table: "BusinessProperties");

            migrationBuilder.DropForeignKey(
                name: "FK_BusinessService_Business_businessId",
                table: "BusinessService");

            migrationBuilder.DropForeignKey(
                name: "FK_BusinessWorkingInfo_Business_businessId",
                table: "BusinessWorkingInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_Campaign_Business_businessId",
                table: "Campaign");

            migrationBuilder.DropForeignKey(
                name: "FK_Complain_User_userId",
                table: "Complain");

            migrationBuilder.DropForeignKey(
                name: "FK_Favorite_Business_businessId",
                table: "Favorite");

            migrationBuilder.DropForeignKey(
                name: "FK_Favorite_User_userId",
                table: "Favorite");

            migrationBuilder.AddColumn<int>(
                name: "workingDayType",
                table: "Business",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessGallery_Business_businessId",
                table: "BusinessGallery",
                column: "businessId",
                principalTable: "Business",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessProperties_Business_businessId",
                table: "BusinessProperties",
                column: "businessId",
                principalTable: "Business",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessService_Business_businessId",
                table: "BusinessService",
                column: "businessId",
                principalTable: "Business",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessWorkingInfo_Business_businessId",
                table: "BusinessWorkingInfo",
                column: "businessId",
                principalTable: "Business",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Campaign_Business_businessId",
                table: "Campaign",
                column: "businessId",
                principalTable: "Business",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Complain_User_userId",
                table: "Complain",
                column: "userId",
                principalTable: "User",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Favorite_Business_businessId",
                table: "Favorite",
                column: "businessId",
                principalTable: "Business",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Favorite_User_userId",
                table: "Favorite",
                column: "userId",
                principalTable: "User",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BusinessGallery_Business_businessId",
                table: "BusinessGallery");

            migrationBuilder.DropForeignKey(
                name: "FK_BusinessProperties_Business_businessId",
                table: "BusinessProperties");

            migrationBuilder.DropForeignKey(
                name: "FK_BusinessService_Business_businessId",
                table: "BusinessService");

            migrationBuilder.DropForeignKey(
                name: "FK_BusinessWorkingInfo_Business_businessId",
                table: "BusinessWorkingInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_Campaign_Business_businessId",
                table: "Campaign");

            migrationBuilder.DropForeignKey(
                name: "FK_Complain_User_userId",
                table: "Complain");

            migrationBuilder.DropForeignKey(
                name: "FK_Favorite_Business_businessId",
                table: "Favorite");

            migrationBuilder.DropForeignKey(
                name: "FK_Favorite_User_userId",
                table: "Favorite");

            migrationBuilder.DropColumn(
                name: "workingDayType",
                table: "Business");

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessGallery_Business_businessId",
                table: "BusinessGallery",
                column: "businessId",
                principalTable: "Business",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessProperties_Business_businessId",
                table: "BusinessProperties",
                column: "businessId",
                principalTable: "Business",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessService_Business_businessId",
                table: "BusinessService",
                column: "businessId",
                principalTable: "Business",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BusinessWorkingInfo_Business_businessId",
                table: "BusinessWorkingInfo",
                column: "businessId",
                principalTable: "Business",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Campaign_Business_businessId",
                table: "Campaign",
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
                name: "FK_Favorite_Business_businessId",
                table: "Favorite",
                column: "businessId",
                principalTable: "Business",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Favorite_User_userId",
                table: "Favorite",
                column: "userId",
                principalTable: "User",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
