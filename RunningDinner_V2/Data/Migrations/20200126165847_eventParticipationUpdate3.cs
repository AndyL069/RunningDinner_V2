using Microsoft.EntityFrameworkCore.Migrations;

namespace RunningDinner.Data.Migrations
{
    public partial class eventParticipationUpdate3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "EventParticipations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddressAdditions",
                table: "EventParticipations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddressNumber",
                table: "EventParticipations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "EventParticipations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "EventParticipations");

            migrationBuilder.DropColumn(
                name: "AddressAdditions",
                table: "EventParticipations");

            migrationBuilder.DropColumn(
                name: "AddressNumber",
                table: "EventParticipations");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "EventParticipations");
        }
    }
}
