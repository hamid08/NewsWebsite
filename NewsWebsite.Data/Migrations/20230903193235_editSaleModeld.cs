using Microsoft.EntityFrameworkCore.Migrations;

namespace NewsWebsite.Data.Migrations
{
    public partial class editSaleModeld : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sales_TransportTerminals_TransportTerminalId",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "TerminalId",
                table: "Sales");

            migrationBuilder.AlterColumn<int>(
                name: "TransportTerminalId",
                table: "Sales",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_TransportTerminals_TransportTerminalId",
                table: "Sales",
                column: "TransportTerminalId",
                principalTable: "TransportTerminals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sales_TransportTerminals_TransportTerminalId",
                table: "Sales");

            migrationBuilder.AlterColumn<int>(
                name: "TransportTerminalId",
                table: "Sales",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "TerminalId",
                table: "Sales",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_TransportTerminals_TransportTerminalId",
                table: "Sales",
                column: "TransportTerminalId",
                principalTable: "TransportTerminals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
