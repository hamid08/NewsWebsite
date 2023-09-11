using Microsoft.EntityFrameworkCore.Migrations;

namespace NewsWebsite.Data.Migrations
{
    public partial class AddIdIntoSiteVisitModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SiteVisits",
                table: "SiteVisits");

            migrationBuilder.AlterColumn<string>(
                name: "IpAddress",
                table: "SiteVisits",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "SiteVisits",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SiteVisits",
                table: "SiteVisits",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SiteVisits",
                table: "SiteVisits");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "SiteVisits");

            migrationBuilder.AlterColumn<string>(
                name: "IpAddress",
                table: "SiteVisits",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SiteVisits",
                table: "SiteVisits",
                column: "IpAddress");
        }
    }
}
