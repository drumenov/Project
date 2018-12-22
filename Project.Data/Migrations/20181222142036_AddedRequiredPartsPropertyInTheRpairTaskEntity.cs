using Microsoft.EntityFrameworkCore.Migrations;

namespace Project.Data.Migrations
{
    public partial class AddedRequiredPartsPropertyInTheRpairTaskEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RepairTaskId",
                table: "Parts",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Parts_RepairTaskId",
                table: "Parts",
                column: "RepairTaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_Parts_RepairTask_RepairTaskId",
                table: "Parts",
                column: "RepairTaskId",
                principalTable: "RepairTask",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Parts_RepairTask_RepairTaskId",
                table: "Parts");

            migrationBuilder.DropIndex(
                name: "IX_Parts_RepairTaskId",
                table: "Parts");

            migrationBuilder.DropColumn(
                name: "RepairTaskId",
                table: "Parts");
        }
    }
}
