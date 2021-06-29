using Microsoft.EntityFrameworkCore.Migrations;

namespace DealerTrack.DataBase.Migrations
{
    public partial class tblDealAddField_DealNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DealNumber",
                table: "Deals",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DealNumber",
                table: "Deals");
        }
    }
}
