using Microsoft.EntityFrameworkCore.Migrations;

namespace NewsWebsite.Data.Migrations
{
    public partial class AddSaleModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TotalFare = table.Column<long>(type: "bigint", nullable: false),
                    DriverValue = table.Column<long>(type: "bigint", nullable: false),
                    TransportationCompanyValue = table.Column<long>(type: "bigint", nullable: false),
                    TransportationUnitValue = table.Column<long>(type: "bigint", nullable: false),
                    SumTotal = table.Column<long>(type: "bigint", nullable: false),
                    TerminalCaption = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sales", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sales");
        }
    }
}
