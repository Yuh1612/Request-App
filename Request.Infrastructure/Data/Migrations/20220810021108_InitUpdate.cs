using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Request.Infrastructure.Data.Migrations
{
    public partial class InitUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "StatusId",
                table: "LeaveRequests",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_LeaveRequests_StatusId",
                table: "LeaveRequests",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveRequests_Statuses_StatusId",
                table: "LeaveRequests",
                column: "StatusId",
                principalTable: "Statuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeaveRequests_Statuses_StatusId",
                table: "LeaveRequests");

            migrationBuilder.DropIndex(
                name: "IX_LeaveRequests_StatusId",
                table: "LeaveRequests");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "LeaveRequests");
        }
    }
}