using Microsoft.EntityFrameworkCore.Migrations;

namespace RunningDinner.Data.Migrations
{
    public partial class TeamsUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SelectedKitchenOption",
                table: "Teams");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Teams",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "Teams");

            migrationBuilder.AddColumn<string>(
                name: "SelectedKitchenOption",
                table: "Teams",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
