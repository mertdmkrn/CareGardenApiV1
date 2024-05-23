using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareGardenApiV1.Migrations
{
    /// <inheritdoc />
    public partial class CareGardenV35 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                        name: "FK_WorkerServicePrice_BusinessService_businessServiceId",
                        column: x => x.businessServiceId,
                        principalTable: "BusinessService",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_WorkerServicePrice_Worker_workerId",
                        column: x => x.workerId,
                        principalTable: "Worker",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkerServicePrice_businessServiceId_workerId",
                table: "WorkerServicePrice",
                columns: new[] { "businessServiceId", "workerId" });

            migrationBuilder.CreateIndex(
                name: "IX_WorkerServicePrice_workerId",
                table: "WorkerServicePrice",
                column: "workerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkerServicePrice");
        }
    }
}
