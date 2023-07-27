using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareGardenApiV1.Migrations
{
    /// <inheritdoc />
    public partial class CareGardenV12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Faq");
        }
    }
}
