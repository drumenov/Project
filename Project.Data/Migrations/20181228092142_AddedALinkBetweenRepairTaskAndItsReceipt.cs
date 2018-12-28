using Microsoft.EntityFrameworkCore.Migrations;

namespace Project.Data.Migrations
{
    public partial class AddedALinkBetweenRepairTaskAndItsReceipt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReceiptId",
                table: "RepairTask",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RepairTask_ReceiptId",
                table: "RepairTask",
                column: "ReceiptId",
                unique: true,
                filter: "[ReceiptId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_RepairTask_Receipts_ReceiptId",
                table: "RepairTask",
                column: "ReceiptId",
                principalTable: "Receipts",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RepairTask_Receipts_ReceiptId",
                table: "RepairTask");

            migrationBuilder.DropIndex(
                name: "IX_RepairTask_ReceiptId",
                table: "RepairTask");

            migrationBuilder.DropColumn(
                name: "ReceiptId",
                table: "RepairTask");
        }
    }
}
