using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DigitalBanking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeInTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FromCardId",
                table: "Transactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ToCardId",
                table: "Transactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_FromCardId",
                table: "Transactions",
                column: "FromCardId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_ToCardId",
                table: "Transactions",
                column: "ToCardId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_VirtualCards_FromCardId",
                table: "Transactions",
                column: "FromCardId",
                principalTable: "VirtualCards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_VirtualCards_ToCardId",
                table: "Transactions",
                column: "ToCardId",
                principalTable: "VirtualCards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_VirtualCards_FromCardId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_VirtualCards_ToCardId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_FromCardId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_ToCardId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "FromCardId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "ToCardId",
                table: "Transactions");
        }
    }
}
