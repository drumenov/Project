using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Project.Data.Migrations
{
    public partial class ChangedAllGuidKeysToStringKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RepairTask_AspNetUsers_UserId1",
                table: "RepairTask");

            migrationBuilder.DropIndex(
                name: "IX_RepairTask_UserId1",
                table: "RepairTask");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "RepairTask");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "RepairTask",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.CreateIndex(
                name: "IX_RepairTask_UserId",
                table: "RepairTask",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_RepairTask_AspNetUsers_UserId",
                table: "RepairTask",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RepairTask_AspNetUsers_UserId",
                table: "RepairTask");

            migrationBuilder.DropIndex(
                name: "IX_RepairTask_UserId",
                table: "RepairTask");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "RepairTask",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

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
    }
}
