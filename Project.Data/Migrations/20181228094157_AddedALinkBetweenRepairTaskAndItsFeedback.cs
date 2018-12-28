using Microsoft.EntityFrameworkCore.Migrations;

namespace Project.Data.Migrations
{
    public partial class AddedALinkBetweenRepairTaskAndItsFeedback : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Parts_RepairTask_RepairTaskId",
                table: "Parts");

            migrationBuilder.DropForeignKey(
                name: "FK_RepairTask_Receipts_ReceiptId",
                table: "RepairTask");

            migrationBuilder.DropForeignKey(
                name: "FK_RepairTask_AspNetUsers_UserId",
                table: "RepairTask");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersRepairsTasks_RepairTask_RepairTaskId",
                table: "UsersRepairsTasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RepairTask",
                table: "RepairTask");

            migrationBuilder.RenameTable(
                name: "RepairTask",
                newName: "RepairTasks");

            migrationBuilder.RenameIndex(
                name: "IX_RepairTask_UserId",
                table: "RepairTasks",
                newName: "IX_RepairTasks_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_RepairTask_ReceiptId",
                table: "RepairTasks",
                newName: "IX_RepairTasks_ReceiptId");

            migrationBuilder.AddColumn<int>(
                name: "RepairTaskId",
                table: "Feedbacks",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FeedbackId",
                table: "RepairTasks",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RepairTasks",
                table: "RepairTasks",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_RepairTaskId",
                table: "Feedbacks",
                column: "RepairTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_RepairTasks_FeedbackId",
                table: "RepairTasks",
                column: "FeedbackId");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_RepairTasks_RepairTaskId",
                table: "Feedbacks",
                column: "RepairTaskId",
                principalTable: "RepairTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Parts_RepairTasks_RepairTaskId",
                table: "Parts",
                column: "RepairTaskId",
                principalTable: "RepairTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RepairTasks_Feedbacks_FeedbackId",
                table: "RepairTasks",
                column: "FeedbackId",
                principalTable: "Feedbacks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RepairTasks_Receipts_ReceiptId",
                table: "RepairTasks",
                column: "ReceiptId",
                principalTable: "Receipts",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_RepairTasks_AspNetUsers_UserId",
                table: "RepairTasks",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersRepairsTasks_RepairTasks_RepairTaskId",
                table: "UsersRepairsTasks",
                column: "RepairTaskId",
                principalTable: "RepairTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_RepairTasks_RepairTaskId",
                table: "Feedbacks");

            migrationBuilder.DropForeignKey(
                name: "FK_Parts_RepairTasks_RepairTaskId",
                table: "Parts");

            migrationBuilder.DropForeignKey(
                name: "FK_RepairTasks_Feedbacks_FeedbackId",
                table: "RepairTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_RepairTasks_Receipts_ReceiptId",
                table: "RepairTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_RepairTasks_AspNetUsers_UserId",
                table: "RepairTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersRepairsTasks_RepairTasks_RepairTaskId",
                table: "UsersRepairsTasks");

            migrationBuilder.DropIndex(
                name: "IX_Feedbacks_RepairTaskId",
                table: "Feedbacks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RepairTasks",
                table: "RepairTasks");

            migrationBuilder.DropIndex(
                name: "IX_RepairTasks_FeedbackId",
                table: "RepairTasks");

            migrationBuilder.DropColumn(
                name: "RepairTaskId",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "FeedbackId",
                table: "RepairTasks");

            migrationBuilder.RenameTable(
                name: "RepairTasks",
                newName: "RepairTask");

            migrationBuilder.RenameIndex(
                name: "IX_RepairTasks_UserId",
                table: "RepairTask",
                newName: "IX_RepairTask_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_RepairTasks_ReceiptId",
                table: "RepairTask",
                newName: "IX_RepairTask_ReceiptId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RepairTask",
                table: "RepairTask",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Parts_RepairTask_RepairTaskId",
                table: "Parts",
                column: "RepairTaskId",
                principalTable: "RepairTask",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RepairTask_Receipts_ReceiptId",
                table: "RepairTask",
                column: "ReceiptId",
                principalTable: "Receipts",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_RepairTask_AspNetUsers_UserId",
                table: "RepairTask",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersRepairsTasks_RepairTask_RepairTaskId",
                table: "UsersRepairsTasks",
                column: "RepairTaskId",
                principalTable: "RepairTask",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
