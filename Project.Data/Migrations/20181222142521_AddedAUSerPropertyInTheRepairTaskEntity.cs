using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Project.Data.Migrations
{
    public partial class AddedAUSerPropertyInTheRepairTaskEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "RepairTask",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "RepairTask",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RepairTask_UserId1",
                table: "RepairTask",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_RepairTask_AspNetUsers_UserId1",
                table: "RepairTask",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RepairTask_AspNetUsers_UserId1",
                table: "RepairTask");

            migrationBuilder.DropIndex(
                name: "IX_RepairTask_UserId1",
                table: "RepairTask");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "RepairTask");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "RepairTask");
        }
    }
}
