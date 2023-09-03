using Microsoft.EntityFrameworkCore.Migrations;

namespace NewsWebsite.Data.Migrations
{
    public partial class addTransportTerminal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DriverValue",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "SumTotal",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "TerminalCaption",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "TransportationCompanyValue",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "TransportationUnitValue",
                table: "Sales");

            migrationBuilder.AddColumn<int>(
                name: "TransportTerminalId",
                table: "Sales",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TripStatus",
                table: "Sales",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TransportTerminals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Caption = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransportTerminals", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sales_TransportTerminalId",
                table: "Sales",
                column: "TransportTerminalId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_TransportTerminals_TransportTerminalId",
                table: "Sales",
                column: "TransportTerminalId",
                principalTable: "TransportTerminals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sales_TransportTerminals_TransportTerminalId",
                table: "Sales");

            migrationBuilder.DropTable(
                name: "TransportTerminals");

            migrationBuilder.DropIndex(
                name: "IX_Sales_TransportTerminalId",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "TransportTerminalId",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "TripStatus",
                table: "Sales");

            migrationBuilder.AddColumn<long>(
                name: "DriverValue",
                table: "Sales",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "SumTotal",
                table: "Sales",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "TerminalCaption",
                table: "Sales",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TransportationCompanyValue",
                table: "Sales",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "TransportationUnitValue",
                table: "Sales",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
